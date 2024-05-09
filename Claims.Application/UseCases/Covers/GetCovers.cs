using Claims.Infrastructure.Repositories;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Covers;

public class GetCovers : IRequestHandler<GetCoversRequest, IEnumerable<Cover>>
{
    private readonly ICoversRepository _coversContext;

    public GetCovers(ICoversRepository coversContext)
    {
        _coversContext = coversContext;
    }

    public async Task<IEnumerable<Cover>> Handle(GetCoversRequest request, CancellationToken cancellationToken)
    {
        return await _coversContext.GetCoversAsync();
    }
}

public record GetCoversRequest() : IRequest<IEnumerable<Cover>>;
