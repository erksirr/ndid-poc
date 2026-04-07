using Microsoft.EntityFrameworkCore;
using ndid_idp.Domain.Entities;

namespace ndid_idp.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<IdpRequest> IdpRequests => Set<IdpRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdpRequest>(e =>
        {
            e.HasKey(x => x.RequestId);
            e.Property(x => x.RequestId).HasMaxLength(100);
            e.Property(x => x.Payload).HasColumnType("jsonb");
            e.Property(x => x.Status).HasMaxLength(20);
        });
    }
}