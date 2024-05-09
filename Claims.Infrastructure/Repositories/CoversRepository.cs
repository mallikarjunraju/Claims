using Claims.Infrastructure.DbContexts;
using Claims.Infrastructure.Exceptions;
using Claims.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Claims.Infrastructure.Repositories;

public class CoversRepository : ICoversRepository
{
    private readonly InsuranceContext _context;
    private readonly ILogger<CoversRepository> _logger;

    public CoversRepository(InsuranceContext context, ILogger<CoversRepository> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task<IEnumerable<Cover>> GetCoversAsync()
    {
        return await _context.Covers.ToListAsync();
    }

    public async Task<Cover> GetCoverByIdAsync(string id)
    {
        var cover = await _context.Covers.FindAsync(id) ?? throw new DataNotFoundException($"Cover not found for id: {id}");

        return cover;
    }

    public async Task<Cover> AddCoverAsync(Cover cover)
    {
        _context.Covers.Add(cover);

        var numOfStateEntries = await _context.SaveChangesAsync();

        if (numOfStateEntries == 0)
        {
            _logger.LogError("Cover not saved");
        }

        return cover;
    }

    public async Task DeleteCoverByIdAsync(string id)
    {
        var cover = await GetCoverByIdAsync(id) ?? throw new DataNotFoundException("Cover not found");

        _context.Covers.Remove(cover);
        await _context.SaveChangesAsync();
    }
}
