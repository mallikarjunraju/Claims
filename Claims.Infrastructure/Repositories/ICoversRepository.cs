using Claims.Models;

namespace Claims.Infrastructure.Repositories;

public interface ICoversRepository
{
    Task<Cover> AddCoverAsync(Cover cover);
    Task<IEnumerable<Cover>> GetCoversAsync();
    Task DeleteCoverByIdAsync(string id);
    Task<Cover> GetCoverByIdAsync(string id);
}