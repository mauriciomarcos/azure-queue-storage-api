using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueueStorageSubscriber.Exceptions;
using QueueStorageSubscriber.Interfaces;

namespace QueueStorageSubscriber.Implementations;

public class QueueSubscriberManager : IQueueSubscriberManager
{
    private readonly string? _connectionString;
    private readonly ILogger<QueueSubscriberManager> _logger;

    public QueueSubscriberManager(IConfiguration appConfiguration,
                                  ILogger<QueueSubscriberManager> logger)
    {
        _connectionString = appConfiguration.GetConnectionString("QueueStorageBroker");
        _logger = logger;
    }

    public async Task DequeueMessagesAsync(string queueName, CancellationToken cancellationToken)
    {
        try
        {
            QueueClient queueClient = CreateQueueClient(queueName);

            if (!queueClient.Exists(cancellationToken))
                throw new DequeueMessageException($"A Fila {queueName} não existe. Operação não realizada!");

            QueueMessage[] receivedMessages = await queueClient.ReceiveMessagesAsync(20, TimeSpan.FromMinutes(1), cancellationToken);
            foreach (var message in receivedMessages)
            {
                _logger.LogInformation("Recuperando a Mensagem {Message} da Fila {QueueName}", message.MessageText, queueName);

                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{ExceptionMessage}", ex.Message);
            throw;
        }      
    }

    private QueueClient CreateQueueClient(string queueName) => new(_connectionString, queueName);
}