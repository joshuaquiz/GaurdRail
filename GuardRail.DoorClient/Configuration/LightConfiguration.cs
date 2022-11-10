﻿using GuardRail.DeviceLogic.Interfaces.Feedback.Lights;

namespace GuardRail.DoorClient.Configuration;

public sealed class LightConfiguration : ILightConfiguration<int>
{
    /// <inheritdoc />
    public int RedLightAddress { get; set; }

    /// <inheritdoc />
    public int GreenLightAddress { get; set; }
}