using Claims.Infrastructure.Repositories;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Claims;

public class GetClaimsHandler(IClaimsRepository claimsContext) : IRequestHandler<GetClaimsRequest, IEnumerable<Claim>>
{
    private readonly IClaimsRepository _claimsContext = claimsContext;

    public async Task<IEnumerable<Claim>> Handle(GetClaimsRequest request, CancellationToken cancellationToken)
    {
        return await _claimsContext.GetClaimsAsync();
    }
}

public record GetClaimsRequest() : IRequest<IEnumerable<Claim>>;
