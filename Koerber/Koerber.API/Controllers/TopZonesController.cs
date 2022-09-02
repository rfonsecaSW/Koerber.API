using Koerber.API.Contracts;
using Koerber.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Koerber.API.Controlers;

[ApiController]
[Route("top-zones")]
public class TopZonesController : ControllerBase
{
    #region Private Fields

    private readonly IKoerberServices _koerberServices;

    private readonly ILogger _logger;

    #endregion Private Fields

    #region Public Constructors

    public TopZonesController(IKoerberServices koerberServices, ILogger<TopZonesController> logger)
    {
        _logger = logger;
        _koerberServices = koerberServices;
    }

    #endregion Public Constructors

    #region Public Methods

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> GetTopZones(string order)
    {
        if (String.IsNullOrWhiteSpace(order))
        {
            return BadRequest("order parameter is null or whitespace");
        }

        TopZonesOutput topZonesOutput = new TopZonesOutput();

        switch (order)
        {
            case "pickups":
                {
                    topZonesOutput = await _koerberServices.GetTopZones(true);
                    break;
                }
            case "dropoffs":
                {
                    topZonesOutput = await _koerberServices.GetTopZones(false);
                    break;
                }
            default:
                {
                    return BadRequest("unrecognized order parameter value");
                }
        }

        if (topZonesOutput.TopZones.Any())
        {
            return Ok(topZonesOutput);
        }

        return NotFound();
    }

    #endregion Public Methods
}