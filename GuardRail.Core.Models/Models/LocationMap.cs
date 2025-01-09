﻿using System;
using System.Collections.Generic;

namespace GuardRail.Core.Models.Models;

/// <summary>
/// A map.
/// </summary>
public class LocationMap
{
    /// <summary>
    /// The global ID for the item.
    /// Guid is to be used in all systems for the global ID.
    /// This value is set automatically and should not be passed in for adds.
    /// </summary>
    public required Guid Guid { get; set; }

    /// <summary>
    /// The ID of the location.
    /// </summary>
    public required Guid LocationGuid { get; set; }

    /// <summary>
    /// The floors in this map.
    /// </summary>
    public List<MapFloor> Floors { get; set; } = [];
}