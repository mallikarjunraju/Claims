using Claims.Infrastructure.DbContexts;
using Claims.Infrastructure.Exceptions;
using Claims.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Repositories;

//Todo : check for exceptions in all methods
public class ClaimsRepository : IClaimsRepository
{
    private readonly InsuranceContext _context;

    public ClaimsRepository(DbContextOptions options, InsuranceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        return await _context.Claims.ToListAsync();
    }

    public async Task<Claim> GetClaimAsync(string id)
    {
        var claim = await _context.Claims.FindAsync(id) ?? throw new DataNotFoundException($"Claim not found for id: {id}");

        return claim;
    }

    // Todo: Check save changes bool
    public async Task<Claim> AddClaimAsync(Claim claim)
    {
        _context.Claims.Add(claim);

        await _context.SaveChangesAsync();

        return claim;
    }

    public async Task DeleteClaimAsync(string id)
    {
        var claim = await GetClaimAsync(id);
        if (claim is not null)
        {
            _context.Claims.Remove(claim);

            await _context.SaveChangesAsync();
        }
    }
}
