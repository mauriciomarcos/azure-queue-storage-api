using Azure.Storage.Queues;

namespace QueueStoragePublisher.Interfaces;

public interface IQueuePublisherManager
{
    Task<bool> CreateQueueIfNotExistsAsync(string queueName);
    Task InsertMessageQueueAsync(string message, string queueName, double? visibilityTimeoutInMinutes = null);
}