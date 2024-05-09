using Claims.Models;

namespace Claims.Infrastructure.Repositories;

public interface IClaimsRepository
{
    Task<IEnumerable<Claim>> GetClaimsAsync();
    Task<Claim> GetClaimAsync(string id);
    Task<Claim> AddClaimAsync(Claim item);
    Task DeleteClaimAsync(string id);
}
