using System;
using GuardRail.Core.EventModels;
using GuardRail.DeviceLogic.Interfaces.Communication;
using GuardRail.DeviceLogic.Interfaces.Door;
using GuardRail.DeviceLogic.Interfaces.Feedback;
using GuardRail.DeviceLogic.Interfaces.Feedback.Buzzer;
using GuardRail.DeviceLogic.Interfaces.Feedback.Lights;
using GuardRail.DeviceLogic.Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GuardRail.DeviceLogic.DependencyHelpers;

/// <summary>
/// Setup extensions for core event handlers.
/// </summary>
public static class CoreEventHandlersSetup
{
    /// <summary>
    /// Sets up the core event listeners.
    /// </summary>
    /// <param name="builder">Used to pull an <see cref="IServiceProvider"/>.</param>
    public static void UseCoreEventHandlers(this IApplicationBuilder builder) =>
        DoorCommandEventHandlers.ConfigureCoreDoorCommandEventHandler(
            builder.ApplicationServices.GetRequiredService<ICentralServerPushCommunication>(),
            builder.ApplicationServices.GetRequiredService<IBuzzerManager>(),
            builder.ApplicationServices.GetRequiredService<ILightManager>(),
            builder.ApplicationServices.GetRequiredService<IDoorManager>(),
            builder.ApplicationServices.GetRequiredService<IScreenManager>(),
            builder.ApplicationServices.GetRequiredService<ILogger<DoorCommand>>());
}