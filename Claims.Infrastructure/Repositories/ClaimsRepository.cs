using Claims.Infrastructure.DbContexts;
using Claims.Infrastructure.Exceptions;
using Claims.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Claims.Infrastructure.Repositories;

public class ClaimsRepository : IClaimsRepository
{
    private readonly InsuranceContext _context;
    private readonly ILogger<ClaimsRepository> _logger;

    public ClaimsRepository(InsuranceContext context, ILogger<ClaimsRepository> logger)
    {
        _context = context;
        _logger = logger;
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

    public async Task<Claim> AddClaimAsync(Claim claim)
    {
        _context.Claims.Add(claim);

        var numOfStateEntries = await _context.SaveChangesAsync();

        if (numOfStateEntries == 0)
        {
            _logger.LogError("Claim not saved");
        }

        return claim;
    }

    public async Task DeleteClaimAsync(string id)
    {
        var claim = await GetClaimAsync(id) ?? throw new DataNotFoundException("Claim not found");

        var entity = _context.Claims.Remove(claim);

        await _context.SaveChangesAsync();
    }
}
