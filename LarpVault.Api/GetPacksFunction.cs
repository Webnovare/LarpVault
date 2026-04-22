using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LarpVault.Api;

public class GetPacksFunction
{
    private readonly ILogger<GetPacksFunction> _logger;

    public GetPacksFunction(ILogger<GetPacksFunction> logger)
    {
        _logger = logger;
    }

    [Function("GetPacks")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "packs")] HttpRequestData req)
    {
        _logger.LogInformation("📋 GetPacks request received");

        try
        {
            var packs = new[]
            {
                new {
                    id = 1,
                    name = "Ultimate LARP Pack",
                    price = 10000,
                    description = "Supercars, luxury watches, private jet vibes, and CEO walk footage",
                    clips = 250
                },
                new {
                    id = 2,
                    name = "Dubai LARP Pack",
                    price = 8500,
                    description = "Burj Khalifa views, Lambo doors, and desert infinity pool shots",
                    clips = 180
                },
                new {
                    id = 3,
                    name = "Crypto Whale Pack",
                    price = 12000,
                    description = "Chart screens, yacht parties, and 'to the moon' energy",
                    clips = 320
                },
                new {
                    id = 4,
                    name = "Founder Grindset Pack",
                    price = 7500,
                    description = "Laptop + coffee + city skyline + 'building in silence' walks",
                    clips = 150
                }
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            await response.WriteAsJsonAsync(new
            {
                success = true,
                count = packs.Length,
                packs = packs
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving packs");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = "Failed to fetch packs" });
            return errorResponse;
        }
    }
}