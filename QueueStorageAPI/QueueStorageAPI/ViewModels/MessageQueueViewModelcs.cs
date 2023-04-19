using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QueueStorageAPI.ViewModels;

public class MessageQueueViewModels
{
    [Required]
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [Required]
    [JsonPropertyName("queueName")]
    public string QueueName { get; } = "user-timeout-queue";

    [Required]
    [JsonPropertyName("timeoutvisibility")]
    public double TimeoutVisibility { get; set; }
}