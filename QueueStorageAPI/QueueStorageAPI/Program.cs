using QueueStorageBackgoundService.Implementations;
using QueueStorageSubscriber.Implementations;
using QueueStorageSubscriber.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IQueueSubscriberManager, QueueSubscriberManager>();
builder.Services.AddHostedService(provider =>
{
    var timeInSeconds = (1000 * 10); // 10 segundos
    var timer = new System.Timers.Timer
    {
        Interval = timeInSeconds,
        AutoReset = true,
        Enabled = true,
    };
    return new BackgroundServiceListener(provider.GetService<IQueueSubscriberManager>()!, timer);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();