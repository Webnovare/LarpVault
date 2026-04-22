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

    [Function("PurchaseFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        _logger.LogInformation("LARP Vault purchase received!");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var purchase = JsonSerializer.Deserialize<PurchaseRequest>(requestBody);

            if (purchase == null || string.IsNullOrEmpty(purchase.Email) || string.IsNullOrEmpty(purchase.PackName))
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteAsJsonAsync(new { message = "Invalid purchase data" });
                return badResponse;
            }

            // TODO: In the next steps we'll save to database + send to queue here

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                success = true,
                message = $"✅ Clout secured! You bought the '{purchase.PackName}' pack.",
                orderId = Guid.NewGuid().ToString(),
                downloadLink = "https://your-larp-vault.com/download/" + Guid.NewGuid()  // placeholder for now
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing purchase");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteAsJsonAsync(new { message = "Something went wrong" });
            return errorResponse;
        }
    }
}

// Simple model for incoming JSON
public class PurchaseRequest
{
    public string? PackName { get; set; }
    public decimal Price { get; set; }
    public string? Email { get; set; }
}