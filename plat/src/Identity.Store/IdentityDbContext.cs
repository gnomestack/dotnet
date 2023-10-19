using GnomeStack.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace GnomeStack.Identity.Store;

public class IdentityDbContext : GsDbContext
{
    public IdentityDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<IdentityTenant> Tenants { get; }

    public DbSet<IdentityTenantDomain> TenantDomains { get; }

    public DbSet<IdentityResource> Resources { get; }

    public DbSet<IdentityPermission> Permissions { get; }

    public DbSet<IdentityUser> Users { get; }

    public DbSet<IdentityRole> Roles { get; }

    public DbSet<IdentityGroup> Groups { get; }

    public DbSet<IdentityServicePrincipal> ServicePrincipals { get; }

    public DbSet<IdentityServicePrincipalKey> ServicePrincipalKeys { get; }

    public DbSet<IdentityApiKey> ApiKeys { get; }

    public DbSet<IdentityService> Services { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}