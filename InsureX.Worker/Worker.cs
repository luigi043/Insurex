using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InsureX.Worker;

public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
{
    private readonly string _apiBaseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:12857/api/compliance";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("InsureX Intelligence Worker started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Worker: Initiating Proactive Compliance Forecasting...");
                
                // In a production scenario, this worker would:
                // 1. Call the ComplianceEngine.RunForecasting() logic.
                // 2. Fetch identified risks from the DB.
                // 3. Dispatch notifications via SendGrid/SMTP.

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Worker: Error occurred during background processing.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        logger.LogInformation("InsureX Intelligence Worker stopped at: {time}", DateTimeOffset.Now);
    }
}
