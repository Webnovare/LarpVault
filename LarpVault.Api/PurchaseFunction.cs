using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace LarpVault.Api;

public class PurchaseFunction
{
    private readonly ILogger<PurchaseFunction> _logger;

    public PurchaseFunction(ILogger<PurchaseFunction> logger)
    {
        _logger = logger;
    }

    [Function("PurchasePack")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "purchase")] HttpRequestData req)
    {
        if (req.Method == "OPTIONS")
        {
            var optionsResponse = req.CreateResponse(HttpStatusCode.NoContent);
            optionsResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            optionsResponse.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            optionsResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return optionsResponse;
        }

        _logger.LogInformation("🏴‍☠️ LARP Purchase request received");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var purchase = JsonSerializer.Deserialize<PurchaseRequest>(requestBody, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (purchase == null || string.IsNullOrWhiteSpace(purchase.Email))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badResponse.WriteAsJsonAsync(new { error = "Email is required" });
                return badResponse;
            }

            var orderId = Guid.NewGuid();

            // Simulate sending to Service Bus Queue
            var queueMessage = new
            {
                OrderId = orderId,
                Email = purchase.Email,
                PackName = purchase.PackName ?? "Ultimate LARP Pack",
                Price = purchase.Price,
                PurchasedAt = DateTime.UtcNow
            };

            _logger.LogInformation("📤 [SIMULATED] Sent Order {OrderId} to Service Bus queue 'larp-purchases'", orderId);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            await response.WriteAsJsonAsync(new
            {
                orderId = orderId,
                packName = purchase.PackName ?? "Ultimate LARP Pack",
                price = purchase.Price,
                status = "Pending",
                message = "🎟️ Purchase successful! Your LARP pack is being processed in the background."
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing purchase");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = "Internal server error" });
            return errorResponse;
        }
    }
}

public class PurchaseRequest
{
    public string Email { get; set; } = string.Empty;
    public string? PackName { get; set; }
    public decimal Price { get; set; } = 10000m;
}