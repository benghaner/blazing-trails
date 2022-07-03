using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.Home.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Features.Home.Shared;

public class GetTrailsEndpoint : BaseAsyncEndpoint.WithRequest<GetTrailsRequest>.WithResponse<GetTrailsRequest.Response>
{
    private readonly BlazingTrailsContext _context;

    public GetTrailsEndpoint(BlazingTrailsContext context)
    {
        _context = context;
    }

    [HttpGet(GetTrailsRequest.RouteTemplate)]
    public override async Task<ActionResult<GetTrailsRequest.Response>> HandleAsync(GetTrailsRequest request,
        CancellationToken cancellationToken = default)
    {
        var trails = await _context
            .Trails
            .ToListAsync(cancellationToken);

        var response = new GetTrailsRequest.Response(trails.Select(t =>
            new GetTrailsRequest.Trail(t.Id, t.Name, t.Location, t.TimeInMinutes, t.Length, t.Description)));

        return Ok(response);
    }
}