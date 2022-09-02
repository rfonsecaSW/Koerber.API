using Koerber.API.Contracts;
using Koerber.Configurations;
using Koerber.DataLoader;
using Koerber.DataLoader.Contracts;
using Koerber.DataReader;
using Koerber.DataReader.Contracts;
using Koerber.DB;
using Koerber.DB.DataModels;

namespace Koerber.API.Services;

public class ZonesServices : IEntityServices<Zones>
{
    #region Private Fields

    private readonly DataLoaderOptions _dataLoaderOptions;

    private readonly IEntityLoader<Zones> _entityLoader;

    private readonly IEntityReader<Zones> _entityReader;

    #endregion Private Fields

    #region Public Constructors

    public ZonesServices(TaxiTripsContext taxiTripsContext, DataLoaderOptions dataLoaderOptions)
    {
        _entityLoader = new ZonesDataLoader(taxiTripsContext);
        _entityReader = new ZonesDataReader();
        _dataLoaderOptions = dataLoaderOptions;
    }

    #endregion Public Constructors

    #region Public Methods

    public void Synchronize()
    {
        IEnumerable<Zones> tripsCollection;

        try
        {
            tripsCollection = _entityReader.Read(_dataLoaderOptions.ZonesFilePath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Unable to read Zones Data. Exception: {ex})");
        }

        if (tripsCollection.Any())
        {
            try
            {
                _entityLoader.Load(tripsCollection);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to load Zones Data. Exception: {ex}");
            }
        }
    }

    #endregion Public Methods
}