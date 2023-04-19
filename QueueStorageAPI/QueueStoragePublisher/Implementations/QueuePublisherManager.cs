using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueueStoragePublisher.Interfaces;

namespace QueueStoragePublisher.Implementations;

public class QueuePublisherManager : IQueuePublisherManager
{
    private readonly string? _connectionString;
    private readonly ILogger<QueuePublisherManager> _logger;

    public QueuePublisherManager(IConfiguration appConfiguration,
                                 ILogger<QueuePublisherManager> logger)
    {
        _connectionString = appConfiguration.GetConnectionString("QueueStorageBroker");
        _logger = logger;
    }

    public async Task<bool> CreateQueueIfNotExistsAsync(string queueName)
    {
        try
        {
            Azure.Response response = null!;
            QueueClient queueClient = CreateQueueClient(queueName);

            if (!queueClient.Exists())
                response = await queueClient.CreateIfNotExistsAsync();

            return response is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar Queue no Azure Storage Account.");
            throw;
        }
    }

    public async Task InsertMessageQueueAsync(string message, string queueName, double? visibilityTimeoutInMinutes = null)
    {
        try
        {
            var timer = visibilityTimeoutInMinutes is not null ?
                        TimeSpan.FromMinutes((double)visibilityTimeoutInMinutes) :
                        TimeSpan.FromMinutes(0);

            QueueClient queueClient = CreateQueueClient(queueName);
            await queueClient.SendMessageAsync(messageText: message, visibilityTimeout: timer);

            _logger.LogInformation("Publicação da Mensagen {Message} na Queue {QueueName} realizada com sucesso.", message, queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar Mensagem {Message} na Queue {QueueName}", message, queueName);
            throw;
        }
    }

    private QueueClient CreateQueueClient(string queueName) => new(_connectionString, queueName);
}