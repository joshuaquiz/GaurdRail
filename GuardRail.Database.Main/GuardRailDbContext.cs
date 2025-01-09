﻿using GuardRail.Core.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace GuardRail.Database.Main;

public class GuardRailDbContext
    : DbContext
{
    public GuardRailDbContext(
        DbContextOptions<GuardRailDbContext> options)
        : base(
            options)
    {

    }

    public DbSet<VersionHistory> VersionHistories { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<EmailTemplate> EmailTemplates { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserAccessToken> UserAccessTokens { get; set; }
}