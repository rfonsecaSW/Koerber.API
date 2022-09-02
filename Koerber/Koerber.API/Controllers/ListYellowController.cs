using Koerber.API.Contracts;
using Koerber.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Koerber.API.Controlers;

[ApiController]
[Route("list-yellow")]
public class ListYellowController : ControllerBase
{
    #region Private Fields

    private readonly IKoerberServices _koerberServices;

    private readonly ILogger _logger;

    #endregion Private Fields

    #region Public Constructors

    public ListYellowController(IKoerberServices koerberServices, ILogger<TopZonesController> logger)
    {
        _logger = logger;
        _koerberServices = koerberServices;
    }

    #endregion Public Constructors

    #region Public Methods

    [HttpGet]
    [HttpPost]
    public async Task<IActionResult> GetYellowTrips(ListYellowInput input)
    {
        IEnumerable<ListYellowOutput> yellowTrips = await _koerberServices.GetListYellow(input);

        return Ok(yellowTrips);
    }

    #endregion Public Methods
}