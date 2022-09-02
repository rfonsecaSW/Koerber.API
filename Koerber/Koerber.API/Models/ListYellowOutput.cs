namespace Koerber.API.Models;

public class ListYellowOutput
{
    #region Public Properties

    public int DropOffLocationID { get; set; }

    public DateTime DropOffTime { get; set; }

    public int PickUpLocationID { get; set; }

    public DateTime PickUpTime { get; set; }

    #endregion Public Properties
}