﻿using System;

namespace GuardRail.Core.Models.Models;

/// <summary>
/// A location/site for an account.
/// </summary>
public class Location
{
    /// <summary>
    /// The global ID for the item.
    /// Guid is to be used in all systems for the global ID.
    /// This value is set automatically and should not be passed in for adds.
    /// </summary>
    public required Guid Guid { get; set; }

    /// <summary>
    /// The ID of the account.
    /// </summary>
    public required Guid AccountGuid { get; set; }

    /// <summary>
    /// The name of this location.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// A description of this location.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The latitude of this location.
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    /// The longitude of this location.
    /// </summary>
    public decimal? Longitude { get; set; }

    /// <summary>
    /// The distance from the lat/long allowed to be considered "in-range".
    /// </summary>
    public decimal? GeoFenceDistance { get; set; }

    /// <summary>
    /// Whether this location is mobile.
    /// </summary>
    public bool IsMobile { get; set; }
}