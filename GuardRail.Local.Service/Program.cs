using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GuardRail.Local.Service;

public static class Program
{
    public static void Main(
        string[] args)
    {
        var builder = Host.CreateApplicationBuilder(
            args);
        builder.Services.AddWindowsService();
        builder.Services.AddHostedService<AutoUpdateCheckerWorker>();
        builder.Services.AddHostedService<UdpPingListenerWorker>();
        var host = builder.Build();
        host.Run();
    }
}