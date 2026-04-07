using Microsoft.EntityFrameworkCore;
using ndid_as.Domain.Entities;

namespace ndid_as.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AsRequest> AsRequests => Set<AsRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AsRequest>(e =>
        {
            e.HasKey(x => x.RequestId);
            e.Property(x => x.RequestId).HasMaxLength(100);
            e.Property(x => x.Payload).HasColumnType("jsonb");
            e.Property(x => x.Status).HasMaxLength(20);
        });
    }
}