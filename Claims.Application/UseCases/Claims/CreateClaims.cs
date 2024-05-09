using Claims.Application.UseCases.Audit;
using Claims.Infrastructure.Exceptions;
using Claims.Infrastructure.Repositories;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Claims;

public class CreateClaims : IRequestHandler<CreateClaimsRequest, Claim>
{
    private readonly IClaimsRepository _claimsRepository;
    private readonly ICoversRepository _coversRepository;

    private readonly IAuditer _auditer;

    public CreateClaims(IClaimsRepository claimsRepository, ICoversRepository coversRepository, IAuditer auditer)
    {
        _claimsRepository = claimsRepository;
        _coversRepository = coversRepository;
        _auditer = auditer;
    }

    public async Task<Claim> Handle(CreateClaimsRequest request, CancellationToken cancellationToken)
    {
        var cover = await _coversRepository.GetCoverByIdAsync(request.Claim.CoverId) ?? throw new DataNotFoundException("Cover not found");

        // Created date must be within the period of the related Cover
        if (request.Claim.Created < cover.StartDate || request.Claim.Created > cover.EndDate)
        {
            throw new RequestValidationException("Claim creation date must be within the period of the related Cover");
        }

        // Todo: Claim Id should be generated and sent back ...should not be present in the request
        string claimId = Guid.NewGuid().ToString();

        request.Claim.Id = claimId;
        var response = await _claimsRepository.AddClaimAsync(request.Claim).ConfigureAwait(false);

        _auditer.AuditClaim(claimId, "POST");

        return response;
    }
}

public record CreateClaimsRequest(Claim Claim) : IRequest<Claim>;
