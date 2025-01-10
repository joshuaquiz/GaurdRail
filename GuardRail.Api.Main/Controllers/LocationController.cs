using GuardRail.Api.Models;
using GuardRail.Api.Models.Requests;
using GuardRail.Core.Models.Models;
using GuardRail.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuardRail.Api.Main.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController(
    ILocationManagementService locationManagementService,
    ILogger<LocationController> logger)
    : GhControllerBase<LocationController>(
        logger)
{
    [HttpPost(Name = nameof(CreateLocation))]
    public async Task<ApiResult> CreateLocation(
        [FromBody]
        CreateNewLocationRequest createNewLocationRequest,
        CancellationToken cancellationToken) =>
        await GhWrappedApiCall(
            async () =>
                await locationManagementService.CreateNewLocation(
                    createNewLocationRequest.AccountId,
                    createNewLocationRequest.IsMobile,
                    createNewLocationRequest.Name,
                    createNewLocationRequest.Description,
                    createNewLocationRequest.Latitude,
                    createNewLocationRequest.Longitude,
                    createNewLocationRequest.GeoFenceDistance,
                    cancellationToken));

    [HttpGet(Name = nameof(ListLocations))]
    public async Task<ApiResult<IReadOnlyCollection<Location>>> ListLocations(
        [FromQuery]
        Guid accountId,
        CancellationToken cancellationToken) =>
        await GhWrappedApiCall(
            async () =>
                await locationManagementService.ListLocations(
                    accountId,
                    cancellationToken));
}