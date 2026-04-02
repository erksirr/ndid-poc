using Microsoft.EntityFrameworkCore;
using ndid_poc.Domain.Entities;

namespace ndid_poc.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<NdidRequest> NdidRequests => Set<NdidRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NdidRequest>(e =>
        {
            e.HasKey(x => x.RequestId);
            e.Property(x => x.RequestId).HasMaxLength(100);
            e.Property(x => x.Payload).HasColumnType("jsonb");
        });
    }
}
