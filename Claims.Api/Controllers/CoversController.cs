using Claims.Application.UseCases.Covers;
using Claims.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("covers")]
public class CoversController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoversController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("compute")]
    public async Task<ActionResult> ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var response = await _mediator.Send(new ComputePremiumRequest(startDate, endDate, coverType));

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetCoversAsync()
    {
        var response = await _mediator.Send(new GetCoversRequest()).ConfigureAwait(false);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetCoverByIdAsync(string id)
    {
        var response = await _mediator.Send(new GetCoverByIdRequest(id)).ConfigureAwait(false);

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Cover>> CreateCoverAsync(Cover cover)
    {
        var response = await _mediator.Send(new CreateCoverRequest(cover)).ConfigureAwait(false);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCoverByIdAsync(string id)
    {
        await _mediator.Send(new DeleteCoverByIdRequest(id)).ConfigureAwait(false);

        return NoContent();
    }
}
