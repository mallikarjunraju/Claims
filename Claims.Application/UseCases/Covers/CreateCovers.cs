using Claims.Application.UseCases.Audit;
using Claims.Application.Utils;
using Claims.Infrastructure.Repositories;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Covers;

public class CreateCovers : IRequestHandler<CreateCoverRequest, Cover>
{
    private readonly ICoversRepository _coversContext;
    private readonly IAuditer _auditer;

    public CreateCovers(ICoversRepository coversContext, IAuditer auditer)
    {
        _coversContext = coversContext;
        _auditer = auditer;
    }

    public async Task<Cover> Handle(CreateCoverRequest request, CancellationToken cancellationToken)
    {
        request.Cover.Id = Guid.NewGuid().ToString();
        request.Cover.Premium = Premium.ComputePremium(request.Cover.StartDate, request.Cover.EndDate, request.Cover.Type);

        Task<Cover> addCoverTask = _coversContext.AddCoverAsync(request.Cover);
        await Task.WhenAll(
            Task.Run(() => _auditer.AuditCover(request.Cover.Id, "POST"), cancellationToken),
                  addCoverTask)
            .ConfigureAwait(false);

        return addCoverTask.Result;
    }
}

public record CreateCoverRequest(Cover Cover) : IRequest<Cover>;
