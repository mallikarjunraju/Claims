using Claims.Infrastructure.Repositories;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Covers;

public class GetCoverById : IRequestHandler<GetCoverByIdRequest, Cover>
{
    private readonly ICoversRepository _coversContext;

    public GetCoverById(ICoversRepository coversContext)
    {
        _coversContext = coversContext;
    }

    public async Task<Cover> Handle(GetCoverByIdRequest request, CancellationToken cancellationToken)
    {
        return await _coversContext.GetCoverByIdAsync(request.Id);
    }
}

public record GetCoverByIdRequest(string Id) : IRequest<Cover>;
