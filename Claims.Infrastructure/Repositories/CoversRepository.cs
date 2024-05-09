using Claims.Infrastructure.DbContexts;
using Claims.Infrastructure.Exceptions;
using Claims.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Repositories;

//Todo : check for exceptions in all methods
public class CoversRepository : ICoversRepository
{
    private readonly InsuranceContext _context;

    public CoversRepository(InsuranceContext context)
    {
        _context = context;
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

    // Todo: Check save changes bool
    public async Task<Cover> AddCoverAsync(Cover cover)
    {
        _context.Covers.Add(cover);
        await _context.SaveChangesAsync();

        return cover;
    }

    public async Task DeleteCoverByIdAsync(string id)
    {
        var cover = await GetCoverByIdAsync(id);

        if (cover is not null)
        {
            _context.Covers.Remove(cover);
            await _context.SaveChangesAsync();
        }
    }
}
