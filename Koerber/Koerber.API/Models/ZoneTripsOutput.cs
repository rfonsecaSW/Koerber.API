using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Koerber.API.Models;

public class CustomDateTimeConverter : IsoDateTimeConverter
{
    #region Public Constructors

    public CustomDateTimeConverter()
    {
        base.DateTimeFormat = "yyyy-MM-dd";
    }

    #endregion Public Constructors
}

public class ZoneTripsOutput
{
    #region Public Properties

    [JsonProperty("date")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime Date { get; set; }

    [JsonProperty("do")]
    public int DropOffTotal { get; set; }

    [JsonProperty("pu")]
    public int PickUpTotal { get; set; }

    [JsonProperty("zone")]
    public string? Zone { get; set; }

    #endregion Public Properties
}