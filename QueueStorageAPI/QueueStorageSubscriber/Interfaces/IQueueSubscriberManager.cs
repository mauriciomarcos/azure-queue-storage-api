namespace QueueStorageSubscriber.Interfaces;

public interface IQueueSubscriberManager
{
    Task DequeueMessagesAsync(string queueName, CancellationToken cancellationToken);
}