using Koerber.DataLoader.Contracts;
using Koerber.DB;
using Koerber.DB.DataModels;

namespace Koerber.DataLoader;

public sealed class ZonesDataLoader : IEntityLoader<Zones>
{
    #region Private Fields

    private readonly TaxiTripsContext _taxiTripsContext;

    #endregion Private Fields

    #region Public Constructors

    public ZonesDataLoader(TaxiTripsContext taxiTripsContext)
    {
        _taxiTripsContext = taxiTripsContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public void Load(IEnumerable<Zones> entityCollection)
    {
        using var transaction = _taxiTripsContext.Database.BeginTransaction();

        try
        {
            _taxiTripsContext.Zones.AddRange(entityCollection);

            _taxiTripsContext.SaveChanges();
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            throw new Exception($"Unable to load Zones Entity Collection. Exception: {ex}");
        }

        transaction.Commit();
    }

    #endregion Public Methods
}