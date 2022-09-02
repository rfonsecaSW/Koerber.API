using Koerber.API.Contracts;
using Koerber.API.Models;
using Koerber.DB;

namespace Koerber.API.Services;

public class KoerberServices : IKoerberServices
{
    #region Private Fields

    private readonly TaxiTripsContext _tripsContext;

    #endregion Private Fields

    #region Public Constructors

    public KoerberServices(TaxiTripsContext tripsContext)
    {
        _tripsContext = tripsContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<IEnumerable<ListYellowOutput>> GetListYellow(ListYellowInput filterOptions)
    {
        IList<ListYellowOutput> listYellow = new List<ListYellowOutput>();

        var tripsQuery = _tripsContext.Trips.AsQueryable();

        tripsQuery = tripsQuery.Skip(filterOptions.Offset);

        if (filterOptions.PickUpDateTimeFilter != null)
        {
            tripsQuery = tripsQuery.Where(trip =>
                trip.PickUpTime >= filterOptions.PickUpDateTimeFilter && trip.PickUpTime <= filterOptions.PickUpDateTimeFilter.Value.AddDays(1)
            );
        }

        if (filterOptions.DropOffDateTimeFilter != null)
        {
            tripsQuery = tripsQuery.Where(trip =>
                trip.DropOffTime >= filterOptions.DropOffDateTimeFilter && trip.DropOffTime <= filterOptions.DropOffDateTimeFilter.Value.AddDays(1)
            );
        }

        if (filterOptions.PickupLocationFilter != null && filterOptions.PickupLocationFilter.Any())
        {
            tripsQuery = tripsQuery.Where(trip =>
                filterOptions.PickupLocationFilter.Any(filter => trip.PickUpLocationID.Equals(filter))
            );
        }

        if (filterOptions.DropOffLocationFilter != null && filterOptions.DropOffLocationFilter.Any())
        {
            tripsQuery = tripsQuery.Where(trip =>
                filterOptions.DropOffLocationFilter.Any(filter => trip.DropOffLocationID.Equals(filter))
            );
        }

        if (filterOptions.Pagination > 0)
        {
            tripsQuery = tripsQuery.Take(filterOptions.Pagination);
        }

        foreach (var trip in tripsQuery.ToList())
        {
            listYellow.Add(new ListYellowOutput()
            {
                PickUpTime = trip.PickUpTime,
                DropOffTime = trip.DropOffTime,
                PickUpLocationID = trip.PickUpLocationID,
                DropOffLocationID = trip.DropOffLocationID
            });
        }

        return listYellow;
    }

    public async Task<TopZonesOutput> GetTopZones(bool sortByPickUps)
    {
        TopZonesOutput topZonesOutput = new TopZonesOutput();

        var pickUpSum = _tripsContext.Trips
            .GroupBy(z => z.PickUpLocationID)
            .Select(group => new { LocationID = group.Key, PickUpCount = group.Count() })
            .ToList();

        var dropOffSum = _tripsContext.Trips
            .GroupBy(z => z.DropOffLocationID)
            .Select(group => new { LocationID = group.Key, DropOffCount = group.Count() })
            .ToList();

        var zoneDefinitions = _tripsContext.Zones.ToList();

        var query = from a in pickUpSum
                    join b in dropOffSum
                            on a.LocationID equals b.LocationID
                    join c in zoneDefinitions
                            on a.LocationID equals c.LocationID
                    select new
                    {
                        a.LocationID,
                        c.Zone,
                        a.PickUpCount,
                        b.DropOffCount
                    };

        query = sortByPickUps ?
            query.OrderByDescending(t => t.PickUpCount).Take(5) :
            query.OrderByDescending(t => t.DropOffCount).Take(5);

        foreach (var result in query)
        {
            topZonesOutput.TopZones.Add(new TopZone()
            {
                Zone = result.Zone,
                PickUpTotal = result.PickUpCount,
                DropOffTotal = result.DropOffCount
            });
        }

        return topZonesOutput;
    }

    public async Task<ZoneTripsOutput> GetZoneTrips(int zoneID, DateTime dateFilter)
    {
        ZoneTripsOutput zoneTripsOutput = new ZoneTripsOutput();

        var pickUpSum = _tripsContext.Trips
            .Where(x =>
                x.PickUpLocationID.Equals(zoneID) &&
                x.PickUpTime >= dateFilter &&
                x.PickUpTime <= dateFilter.AddDays(1)
            )
            .GroupBy(z => z.PickUpLocationID)
            .Select(group => new { LocationID = group.Key, PickUpCount = group.Count() })
            .ToList();

        var dropOffSum = _tripsContext.Trips
            .Where(x =>
                x.DropOffLocationID.Equals(zoneID) &&
                x.DropOffTime >= dateFilter &&
                x.DropOffTime <= dateFilter.AddDays(1)
            )
            .GroupBy(z => z.DropOffLocationID)
            .Select(group => new { LocationID = group.Key, DropOffCount = group.Count() })
            .ToList();

        var zoneDefinitions = _tripsContext.Zones.ToList();

        var query = from a in pickUpSum
                    join b in dropOffSum
                        on a.LocationID equals b.LocationID
                    join c in zoneDefinitions
                        on a.LocationID equals c.LocationID
                    select new
                    {
                        a.LocationID,
                        c.Zone,
                        a.PickUpCount,
                        b.DropOffCount
                    };

        if (query.Any())
        {
            zoneTripsOutput = new ZoneTripsOutput()
            {
                Zone = query.First().Zone,
                Date = dateFilter,
                DropOffTotal = query.First().DropOffCount,
                PickUpTotal = query.First().PickUpCount,
            };
        }

        return zoneTripsOutput;
    }

    public async Task<bool> IsZoneAvailable(int zoneID)
    {
        var zone = _tripsContext.Zones.Where(z => z.LocationID.Equals(zoneID)).FirstOrDefault();

        return zone is null ? false : true;
    }

    #endregion Public Methods
}