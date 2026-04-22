using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using LarpVault.Data;

namespace LarpVault.Api;

public class PurchaseProcessor
{
    private readonly ILogger<PurchaseProcessor> _logger;

    public PurchaseProcessor(ILogger<PurchaseProcessor> logger)
    {
        _logger = logger;
    }

    [Function("PurchaseProcessor")]
    public async Task Run(
        [ServiceBusTrigger("larp-purchases", Connection = "ServiceBusConnectionString")] string message,
        FunctionContext context)
    {
        _logger.LogInformation("🎟️ Processing LARP purchase from queue: {Message}", message);

        try
        {
            var purchaseMessage = System.Text.Json.JsonSerializer.Deserialize<PurchaseMessage>(message);

            if (purchaseMessage == null)
            {
                _logger.LogWarning("Invalid message received from queue");
                return;
            }

            _logger.LogInformation("✅ Delivering LARP pack to {Email} | Pack: {PackName} | OrderId: {OrderId}",
                purchaseMessage.Email, purchaseMessage.PackName, purchaseMessage.OrderId);

            // Simulate delivery processing
            await Task.Delay(2000); // Simulate work

            _logger.LogInformation("🎉 LARP Pack delivered successfully to {Email}! Download link ready.", purchaseMessage.Email);

            // In a real app, you could:
            // - Update status in SQL Database
            // - Send email with download link
            // - Push to another queue for notification
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process purchase message");
            // Here you would typically send to Dead Letter Queue automatically
        }
    }
}

public class PurchaseMessage
{
    public Guid OrderId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PackName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}