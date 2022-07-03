using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Features.ManageTrails.EditTrail;

public class EditTrailEndpoint : BaseAsyncEndpoint.WithRequest<EditTrailRequest>.WithResponse<bool>
{
    private readonly BlazingTrailsContext _context;

    public EditTrailEndpoint(BlazingTrailsContext context)
    {
        _context = context;
    }

    [HttpPut(EditTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<bool>> HandleAsync(EditTrailRequest request,
        CancellationToken cancellationToken = default)
    {
        var trail = await _context.Trails
            .Include(t => t.Route)
            .SingleOrDefaultAsync(t => t.Id == request.Trail.Id, cancellationToken);

        if (trail == null)
            return BadRequest("Trail could not be found");

        trail.Name = request.Trail.Name;
        trail.Description = request.Trail.Description;
        trail.Location = request.Trail.Location;
        trail.TimeInMinutes = request.Trail.TimeInMinutes;
        trail.Length = request.Trail.Length;
        trail.Route = request.Trail.Route.Select(ri => new RouteInstruction
        {
            Stage = ri.Stage,
            Description = ri.Description,
            Trail = trail
        }).ToList();

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(true);
    }
}