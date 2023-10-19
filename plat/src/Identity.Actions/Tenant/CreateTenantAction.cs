using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;

using GnomeStack.Functional;
using GnomeStack.Identity.Actions.Events;
using GnomeStack.Identity.Store;
using GnomeStack.Text.Json;
using Humanizer;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging ;

using StringExtensions = GnomeStack.Extra.Strings.StringExtensions;

namespace GnomeStack.Identity.Actions.Tenant;

#pragma warning disable SA1135
#pragma warning disable SA1200
using CreateResult = Result<Guid, Error>;

public class CreateTenantAction : ICommand<CreateResult>
{
    public Guid Uid { get; set; }

    public string Name { get; set; } = string.Empty;

    public HashSet<string> Domains { get; set; } = new();
}

public class CreateTenantActionHandler : AuditedAction, ICommandHandler<CreateTenantAction, CreateResult>
{
    public CreateTenantActionHandler(
        IdentityDbContext db,
        IIdentityAuditEventLog auditLog,
        ILogger<CreateTenantActionHandler> logger,
        IJsonSerializer jsonSerializer)
        : base("tenant_creation", db, logger, auditLog, jsonSerializer)
    {
    }

    public async ValueTask<CreateResult> Handle(CreateTenantAction request, CancellationToken cancellationToken)
    {
        using var activity = Otel.ActivitySource.CreateActivity("tenant_create", ActivityKind.Internal);
        activity?.AddTag("name", request.Name);

        if (StringExtensions.IsNullOrWhiteSpace(request.Name))
        {
            return new Error("Tenant name is required")
            {
                Code = "tenant:name_required",
            };
        }

        var table = this.Db.Set<IdentityTenant>();
        var slug = request.Name.Underscore().Dasherize();
        var count = await table.CountAsync(o => o.Slug == slug, cancellationToken)
            .NoCap();

        if (count > 0)
        {
            return new Error($"Tenant {request.Name} already exists")
            {
                Code = "tenant:already_exists",
            };
        }

        var tenant = new IdentityTenant
        {
            TenantId = Guid.NewGuid(),
            Name = "Test Tenant",
            Slug = slug,
        };

        var domains = new HashSet<IdentityTenantDomain>();
        foreach (var domain in request.Domains)
        {
            domains.Add(new IdentityTenantDomain
            {
                Tenant = tenant,
                Domain = domain,
            });
        }

        this.Db.Add(tenant);
        this.Db.AddRange(domains);

        var e = await this.SaveChangesAsync(tenant, "Failed to create tenant", cancellationToken)
            .NoCap();

        activity?.Stop();

        if (e is not null)
            return e;

        return tenant.TenantId;
    }
}