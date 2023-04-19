using Microsoft.AspNetCore.Mvc;
using QueueStorageAPI.ViewModels;
using QueueStoragePublisher.Interfaces;

namespace QueueStorageAPI.Controllers;

[ApiController]
[Route("api/storage-queue")]
public class StorageQueueController : ControllerBase
{
    private readonly IQueuePublisherManager _publisher;

    public StorageQueueController(IQueuePublisherManager publisher) => _publisher = publisher;

    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] MessageQueueViewModelcs messageQueue)
    {
        await _publisher.InsertMessageQueueAsync(messageQueue.Message, messageQueue.QueueName, messageQueue.TimeoutVisibility);
        return Accepted(new { success = "Mensagem adicionada na fila com sucesso!" });
    }
}