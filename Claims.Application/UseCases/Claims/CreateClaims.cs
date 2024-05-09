using Claims.Application.UseCases.Audit;
using Claims.Infrastructure.Exceptions;
using Claims.Infrastructure.Repositories;
using Claims.Infrastructure.Services;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Claims;

public class CreateClaims : IRequestHandler<CreateClaimsRequest, Claim>
{
    private readonly IClaimsRepository _claimsRepository;
    private readonly ICoversRepository _coversRepository;
    private readonly IAuditer _auditer;
    private readonly IGlobalIdGenerator _globalIdGenerator;

    public CreateClaims(
        IClaimsRepository claimsRepository,
        ICoversRepository coversRepository,
        IAuditer auditer,
        IGlobalIdGenerator globalIdGenerator)
    {
        _claimsRepository = claimsRepository;
        _coversRepository = coversRepository;
        _auditer = auditer;
        _globalIdGenerator = globalIdGenerator;
    }

    public async Task<Claim> Handle(CreateClaimsRequest request, CancellationToken cancellationToken)
    {
        string claimId = _globalIdGenerator.GenerateId().ToString();

        var cover = await _coversRepository.GetCoverByIdAsync(request.Claim.CoverId) ?? throw new DataNotFoundException("Cover not found");

        // Created date must be within the period of the related Cover
        if (request.Claim.Created < cover.StartDate || request.Claim.Created > cover.EndDate)
        {
            throw new RequestValidationException("Claim creation date must be within the period of the related Cover");
        }

        request.Claim.Id = claimId;
        var claimTask = _claimsRepository.AddClaimAsync(request.Claim);

        await Task.WhenAll(
            Task.Run(() => _auditer.AuditClaim(claimId, "POST"), cancellationToken),
            claimTask)
            .ConfigureAwait(false);

        return claimTask.Result;
    }
}

public record CreateClaimsRequest(Claim Claim) : IRequest<Claim>;
