using Newtonsoft.Json;

namespace Koerber.API.Models;

public sealed class TopZone
{
    #region Public Properties

    [JsonProperty("do_total")]
    public int DropOffTotal { get; set; }

    [JsonProperty("pu_total")]
    public int PickUpTotal { get; set; }

    [JsonProperty("zone")]
    public string? Zone { get; set; }

    #endregion Public Properties
}

public sealed class TopZonesOutput
{
    #region Public Constructors

    public TopZonesOutput()
    {
        TopZones = new List<TopZone>();
    }

    #endregion Public Constructors

    #region Public Properties

    [JsonProperty("top-zones")]
    public IList<TopZone> TopZones { get; set; }

    #endregion Public Properties
}