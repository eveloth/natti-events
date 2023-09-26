using Microsoft.EntityFrameworkCore;

namespace NattiEvents.Data;

public class NattiEventsContext : DbContext
{
    public NattiEventsContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IAssemblyMarker).Assembly);
    }
}
