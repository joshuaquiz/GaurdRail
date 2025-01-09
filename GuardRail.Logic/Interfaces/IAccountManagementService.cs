﻿using System.Threading;
using System.Threading.Tasks;

namespace GuardRail.Logic.Interfaces;

/// <summary>
/// An interface defining high-level actions for account management.
/// </summary>
public interface IAccountManagementService
{
    /// <summary>
    /// Creates a new account and new root user for that account.
    /// </summary>
    /// <param name="accountName">The name of the new account.</param>
    /// <param name="firstName">The first name of the new user.</param>
    /// <param name="lastName">The last name of the new user.</param>
    /// <param name="phone">The users phone.</param>
    /// <param name="email">The users email.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="Task"/> representing the work to create the new account and new user.</returns>
    public Task CreateNewAccount(
        string accountName,
        string firstName,
        string lastName,
        string phone,
        string email,
        CancellationToken cancellationToken);
}