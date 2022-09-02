using Koerber.API.Models;

namespace Koerber.API.Contracts;

public interface IKoerberServices
{
    #region Public Methods

    /// <summary>
    /// Returns trips data by filtering options
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="offset"></param>
    /// <param name="PickUpDateTimeFilter"></param>
    /// <param name="DropOffDateTimeFilter"></param>
    /// <param name="DropOffLocationFilter"></param>
    /// <param name="PickupLocationFilter"></param>
    /// <returns></returns>
    Task<IEnumerable<ListYellowOutput>> GetListYellow(ListYellowInput filterOptions);

    /// <summary>
    /// Return a list of the first 5 zones order by number of total pickups or the number of total drop-offs
    /// </summary>
    /// <param name="sortByPickUps"></param>
    /// <returns></returns>
    Task<TopZonesOutput> GetTopZones(bool sortByPickUps);

    /// <summary>
    /// Return the sum of the pickups and drop-offs in just one zone and one date
    /// </summary>
    /// <param name="zoneID"></param>
    /// <param name="dateFilter"></param>
    /// <returns></returns>
    Task<ZoneTripsOutput> GetZoneTrips(int zoneID, DateTime dateFilter);

    /// <summary>
    /// Returns if the specified zone is available
    /// </summary>
    /// <param name="zoneID"></param>
    /// <returns></returns>
    Task<bool> IsZoneAvailable(int zoneID);

    #endregion Public Methods
}