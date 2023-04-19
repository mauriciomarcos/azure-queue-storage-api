using Microsoft.Extensions.Hosting;
using QueueStorageSubscriber.Interfaces;

namespace QueueStorageBackgoundService.Implementations;

public sealed class BackgroundServiceListener : IHostedService, IDisposable
{
    private readonly IQueueSubscriberManager _queueListener;
    private readonly System.Timers.Timer _timer;

    public BackgroundServiceListener(IQueueSubscriberManager queueListener,
                                     System.Timers.Timer timer) => (_queueListener, _timer) = (queueListener, timer);

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer.Elapsed += async (sender, e) => await _queueListener.DequeueMessagesAsync("user-timeout-queue", cancellationToken);
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken) => await Task.CompletedTask;

    public void Dispose() => _timer?.Dispose();
}