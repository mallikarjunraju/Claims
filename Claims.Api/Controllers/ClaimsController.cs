using Claims.Application.UseCases.Claims;
using Claims.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("claims")]
public class ClaimsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClaimsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Claim>>> GetClaimsAsync()
    {
        var claims = await _mediator.Send(new GetClaimsRequest()).ConfigureAwait(false);

        return Ok(claims);
    }

    [HttpPost]
    public async Task<ActionResult<Claim>> CreateClaimAsync(Claim claim)
    {
        var response = await _mediator.Send(new CreateClaimsRequest(claim)).ConfigureAwait(false);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteClaimByIdAsync(string id)
    {
        await _mediator.Send(new DeleteClaimByIdRequest(id)).ConfigureAwait(false);

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Claim>> GetClaimByIdAsync(string id)
    {
        var response = await _mediator.Send(new GetClaimByIdRequest(id)).ConfigureAwait(false);

        return Ok(response);
    }
}
