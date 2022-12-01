﻿using System.Threading;
using System.Threading.Tasks;
using GuardRail.Api.Data;
using GuardRail.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuardRail.Api.Controllers;

[AllowAnonymous]
[Route("api/setup")]
[ApiController]
public sealed class SetupController : ControllerBase
{
    private readonly GuardRailContext _guardRailContext;

    public SetupController(
        GuardRailContext guardRailContext)
    {
        _guardRailContext = guardRailContext;
    }

    [HttpPost]
    public async Task<bool> SetupAsync(
        SetupModel setupModel,
        CancellationToken cancellationToken)
    {
        if (await _guardRailContext.Users.AnyAsync(cancellationToken))
        {
            return false;
        }

        await _guardRailContext.Users.AddAsync(
            new User
            {
                Email = setupModel.Username,
                Username = setupModel.Username,
                Password = setupModel.Password,
                FirstName = setupModel.FirstName,
                LastName = setupModel.LastName
            },
            cancellationToken);
        await _guardRailContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}