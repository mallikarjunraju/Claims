using Claims.Infrastructure.Repositories;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Claims;

public class GetClaimById : IRequestHandler<GetClaimByIdRequest, Claim>
{
    private readonly IClaimsRepository _claimsContext;

    public GetClaimById(IClaimsRepository claimsContext)
    {
        _claimsContext = claimsContext;
    }

    public async Task<Claim> Handle(GetClaimByIdRequest request, CancellationToken cancellationToken)
    {
        return await _claimsContext.GetClaimAsync(request.Id);
    }
}

public record GetClaimByIdRequest(string Id) : IRequest<Claim>;
