﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using GuardRail.Api.Data;
using GuardRail.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GuardRail.Api
{
    public sealed class DefaultEventBus : IEventBus
    {
        public ObservableCollection<AccessAuthorizationEvent> AccessAuthorizationEvents { get; }

        public DefaultEventBus(
            IServiceProvider serviceProvider)
        {
            AccessAuthorizationEvents = new ObservableCollection<AccessAuthorizationEvent>();
            AccessAuthorizationEvents.CollectionChanged += async (sender, args) =>
            {
                if (args.Action != NotifyCollectionChangedAction.Add)
                {
                    return;
                }

                var authorizer = serviceProvider.GetRequiredService<IAuthorizer>();
                var guardRailContext = serviceProvider.GetRequiredService<GuardRailContext>();
                var guardRailLogger = serviceProvider.GetRequiredService<GuardRailLogger>();
                foreach (AccessAuthorizationEvent newItem in args.NewItems)
                {
                    var acdId = await newItem.AccessControlDevice.GetDeviceId();
                    var accessControlDevice =
                        await guardRailContext.AccessControlDevices.SingleOrDefaultAsync(
                            x => x.DeviceId == acdId);
                    if (!accessControlDevice.IsConfigured)
                    {
                        var log = $"Access control device {(string.IsNullOrWhiteSpace(accessControlDevice.FriendlyName) ? accessControlDevice.DeviceId : accessControlDevice.FriendlyName)} is not configured";
                        await guardRailLogger.LogAsync(log);
                        await newItem.AccessControlDevice.PresentNoAccessGranted(log);
                        AccessAuthorizationEvents.Remove(newItem);
                        return;
                    }

                    var device =
                        await guardRailContext.Devices.SingleOrDefaultAsync(
                            x => x.DeviceId == newItem.Device.DeviceId);
                    if (device == null)
                    {
                        var log = $"Device {(string.IsNullOrWhiteSpace(newItem.Device.FriendlyName) ? newItem.Device.DeviceId : newItem.Device.FriendlyName)} is not configured";
                        await guardRailContext.Devices.AddAsync(
                            new Device
                            {
                                ByteId = newItem.Device.ByteId,
                                DeviceId = newItem.Device.DeviceId,
                                FriendlyName = newItem.Device.FriendlyName,
                                IsConfigured = false
                            });
                        await guardRailContext.SaveChangesAsync();
                        await guardRailLogger.LogAsync(log);
                        await newItem.AccessControlDevice.PresentNoAccessGranted(log);
                        AccessAuthorizationEvents.Remove(newItem);
                        return;
                    }

                    if (!device.IsConfigured)
                    {
                        var log = $"Device {(string.IsNullOrWhiteSpace(device.FriendlyName) ? device.DeviceId : device.FriendlyName)} is not configured";
                        await guardRailLogger.LogAsync(log);
                        await newItem.AccessControlDevice.PresentNoAccessGranted(log);
                        AccessAuthorizationEvents.Remove(newItem);
                        return;
                    }

                    if (await authorizer.IsDeviceAuthorizedAtLocation(
                        device.User,
                        newItem.AccessControlDevice))
                    {
                        foreach (var door in accessControlDevice.Doors)
                        {
                            await door.UnLockAsync(CancellationToken.None);
                        }
                    }
                    else
                    {
                        var log = $"{device.User.FirstName} {device.User.LastName} is not allowed entry at {accessControlDevice.FriendlyName}";
                        await guardRailLogger.LogAsync(log);
                        await newItem
                            .AccessControlDevice
                            .PresentNoAccessGranted(
                                log);
                    }
                    AccessAuthorizationEvents.Remove(newItem);
                }
            };
        }
    }
}