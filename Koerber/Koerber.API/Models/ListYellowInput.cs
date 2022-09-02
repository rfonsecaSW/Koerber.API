namespace Koerber.API.Models;

public class ListYellowInput
{
    #region Public Properties

    public DateTime? DropOffDateTimeFilter { get; set; }
    public List<int>? DropOffLocationFilter { get; set; }
    public int Offset { get; set; }
    public int Pagination { get; set; }
    public DateTime? PickUpDateTimeFilter { get; set; }
    public List<int>? PickupLocationFilter { get; set; }

    #endregion Public Properties
}