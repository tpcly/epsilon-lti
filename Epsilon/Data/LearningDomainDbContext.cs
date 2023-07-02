using Epsilon.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Epsilon.Data;

public class LearningDomainDbContext : DbContext
{
    public LearningDomainDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<LearningDomain> LearningDomains { get; set; } = null!;

    public DbSet<LearningDomainType> LearningDomainTypes { get; set; } = null!;

    public DbSet<LearningDomainOutcome> LearningDomainOutcomes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LearningDomainTypeSet>()
                    .HasMany(static e => e.Types)
                    .WithMany(static e => e.Sets)
                    .UsingEntity(static join => join.ToTable("LearningDomainTypeSetTypes"));

        modelBuilder.Entity<LearningDomainOutcome>()
                    .HasKey(static o => new { o.Id, o.TenantId, });
    }
}