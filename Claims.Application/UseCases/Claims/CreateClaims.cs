using Claims.Application.UseCases.Audit;
using Claims.Infrastructure.Repositories;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Claims;

public class CreateClaims : IRequestHandler<CreateClaimsRequest, Claim>
{
    private readonly IClaimsRepository _claimsContext;
    private readonly IAuditer _auditer;

    public CreateClaims(IClaimsRepository claimsContext, IAuditer auditer)
    {
        _claimsContext = claimsContext;
        _auditer = auditer;
    }

    public async Task<Claim> Handle(CreateClaimsRequest request, CancellationToken cancellationToken)
    {
        // Todo: Claim Id should be generated and sent back ...should not be present in the request
        string claimId = Guid.NewGuid().ToString();

        request.Claim.Id = claimId;
        var response = await _claimsContext.AddClaimAsync(request.Claim).ConfigureAwait(false);

        _auditer.AuditClaim(claimId, "POST");

        return response;
    }
}

public record CreateClaimsRequest(Claim Claim) : IRequest<Claim>;
