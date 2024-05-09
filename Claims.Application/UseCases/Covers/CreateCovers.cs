using Claims.Application.UseCases.Audit;
using Claims.Application.Utils;
using Claims.Infrastructure.Repositories;
using Claims.Infrastructure.Services;
using Claims.Models;
using MediatR;

namespace Claims.Application.UseCases.Covers;

public class CreateCovers : IRequestHandler<CreateCoverRequest, Cover>
{
    private readonly ICoversRepository _coversContext;
    private readonly IAuditer _auditer;
    private readonly IGlobalIdGenerator _globalIdGenerator;

    public CreateCovers(ICoversRepository coversContext, IAuditer auditer, IGlobalIdGenerator globalIdGenerator)
    {
        _coversContext = coversContext;
        _auditer = auditer;
        _globalIdGenerator = globalIdGenerator;
    }

    public async Task<Cover> Handle(CreateCoverRequest request, CancellationToken cancellationToken)
    {
        request.Cover.Id = _globalIdGenerator.GenerateId().ToString();
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
