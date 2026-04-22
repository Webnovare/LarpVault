using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using LarpVault.Data;
using Microsoft.EntityFrameworkCore;

namespace LarpVault.Api;

public class PurchaseFunction
{
    private readonly LarpVaultDbContext _db;
    private readonly ILogger<PurchaseFunction> _logger;

    public PurchaseFunction(LarpVaultDbContext db, ILogger<PurchaseFunction> logger)
    {
        _db = db;
        _logger = logger;
    }

    [Function("PurchasePack")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "purchase")] HttpRequestData req)
    {
        _logger.LogInformation("Purchase request received at {Time}", DateTime.UtcNow);

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var purchase = System.Text.Json.JsonSerializer.Deserialize<PurchaseRequest>(requestBody);

            if (purchase == null || string.IsNullOrWhiteSpace(purchase.Email))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { error = "Email is required" });
                return badResponse;
            }

            var order = new PurchaseOrder
            {
                CustomerEmail = purchase.Email,
                PackName = purchase.PackName ?? "Ultimate LARP Pack",
                Price = purchase.Price
            };

            _db.PurchaseOrders.Add(order);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} saved to database for {Email}", order.Id, purchase.Email);

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(new
            {
                orderId = order.Id,
                packName = order.PackName,
                price = order.Price,
                status = "Pending",
                message = "🎟️ Purchase recorded successfully! Your LARP pack is being processed."
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing purchase for email: {Email}", 
                System.Text.Json.JsonSerializer.Deserialize<PurchaseRequest>(await new StreamReader(req.Body).ReadToEndAsync())?.Email ?? "unknown");

            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
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