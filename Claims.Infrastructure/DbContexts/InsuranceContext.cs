using Claims.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Infrastructure.DbContexts;

public class InsuranceContext : DbContext
{
    private const string ClaimsCollection = "claims";
    private const string CoversCollection = "covers";

    public DbSet<Claim> Claims { get; set; }
    public DbSet<Cover> Covers { get; set; }

    public InsuranceContext(DbContextOptions<InsuranceContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Claim>().ToCollection(ClaimsCollection);
        modelBuilder.Entity<Cover>().ToCollection(CoversCollection);
    }
}
