using Claims.Application.Utils;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Covers;

public class ComputePremium : IRequestHandler<ComputePremiumRequest, decimal>
{
    public Task<decimal> Handle(ComputePremiumRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Premium.ComputePremium(request.StartDate, request.EndDate, request.CoverType));
    }
}

public record ComputePremiumRequest(DateTime StartDate, DateTime EndDate, CoverType CoverType) : IRequest<decimal>;
