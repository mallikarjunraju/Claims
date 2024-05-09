using Claims.Application.UseCases.Audit;
using Claims.Infrastructure.Repositories;
using MediatR;

namespace Claims.Application.UseCases.Claims;

public class DeleteClaimById : IRequestHandler<DeleteClaimByIdRequest, Unit>
{
    private readonly IClaimsRepository _claimsContext;
    private readonly IAuditer _auditer;

    public DeleteClaimById(IClaimsRepository claimsContext, IAuditer auditer)
    {
        _claimsContext = claimsContext;
        _auditer = auditer;
    }

    public async Task<Unit> Handle(DeleteClaimByIdRequest request, CancellationToken cancellationToken)
    {
        _auditer.AuditClaim(request.Id, "DELETE");

        await _claimsContext.DeleteClaimAsync(request.Id);

        return Unit.Value;
    }
}

public record DeleteClaimByIdRequest(string Id) : IRequest<Unit>;