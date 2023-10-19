using Microsoft.EntityFrameworkCore;

namespace GnomeStack.EntityFrameworkCore;

public abstract class GsDbContext : DbContext
{
    protected GsDbContext(DbContextOptions options)
        : base(options)
    {
    }
}