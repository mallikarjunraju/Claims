using Claims.Application.UseCases.Audit;
using Claims.Infrastructure.Repositories;
using MediatR;

namespace Claims.Application.UseCases.Covers;

public class DeleteCoverById : IRequestHandler<DeleteCoverByIdRequest, Unit>
{
    private readonly ICoversRepository _coversContext;
    private readonly IAuditer _auditer;

    public DeleteCoverById(ICoversRepository coversContext, IAuditer auditer)
    {
        _coversContext = coversContext;
        _auditer = auditer;
    }

    public async Task<Unit> Handle(DeleteCoverByIdRequest request, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
            Task.Run(() => _auditer.AuditCover(request.Id, "DELETE"), cancellationToken),
                  _coversContext.DeleteCoverByIdAsync(request.Id))
            .ConfigureAwait(false);

        return Unit.Value;
    }
}

public record DeleteCoverByIdRequest(string Id) : IRequest<Unit>;
