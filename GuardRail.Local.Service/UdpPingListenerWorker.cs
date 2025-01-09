using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GuardRail.Local.Service;

public sealed class UdpPingListenerWorker : BackgroundService
{
    private readonly ILogger<UdpPingListenerWorker> _logger;

    public UdpPingListenerWorker(
        ILogger<UdpPingListenerWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("UdpPingListenerWorker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}