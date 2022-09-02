using Koerber.API.Contracts;
using Koerber.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Koerber.API.Controlers;

[ApiController]
[Route("zone-trips")]
public class ZoneTripsController : ControllerBase
{
    #region Private Fields

    private readonly IKoerberServices _koerberServices;

    private readonly ILogger _logger;

    #endregion Private Fields

    #region Public Constructors

    public ZoneTripsController(IKoerberServices koerberServices, ILogger<TopZonesController> logger)
    {
        _logger = logger;
        _koerberServices = koerberServices;
    }

    #endregion Public Constructors

    #region Public Methods

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> GetZoneTrips(int zone, DateTime date)
    {
        var isZoneAvailable = await _koerberServices.IsZoneAvailable(zone);

        if (!isZoneAvailable)
        {
            return NotFound($"Zone {zone} is not available");
        }

        ZoneTripsOutput zoneTripsOutput = await _koerberServices.GetZoneTrips(zone, date);

        if (zoneTripsOutput == null)
        {
            return NotFound($"No trips are available for Zone {zone}");
        }

        return Ok(zoneTripsOutput);
    }

    #endregion Public Methods
}