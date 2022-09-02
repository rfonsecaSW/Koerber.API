using Koerber.API.Contracts;
using Koerber.Configurations;
using Koerber.DataLoader;
using Koerber.DataLoader.Contracts;
using Koerber.DataReader;
using Koerber.DataReader.Contracts;
using Koerber.DB;
using Koerber.DB.DataModels;

namespace Koerber.API.Services
{
    public class GreenTaxiServices : IEntityServices<Trips>
    {
        #region Private Fields

        private readonly DataLoaderOptions _dataLoaderOptions;

        private readonly IEntityLoader<Trips> _entityLoader;

        private readonly IEntityReader<Trips> _entityReader;

        #endregion Private Fields

        #region Public Constructors

        public GreenTaxiServices(TaxiTripsContext taxiTripsContext, DataLoaderOptions dataLoaderOptions)
        {
            _entityLoader = new TripsDataLoader(taxiTripsContext);
            _entityReader = new GreenTripsDataReader();
            _dataLoaderOptions = dataLoaderOptions;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Synchronize()
        {
            IEnumerable<Trips> tripsCollection;

            try
            {
                tripsCollection = _entityReader.Read(_dataLoaderOptions.GreenTaxiFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to read Green Taxi Data. Exception: {ex})");
            }

            if (tripsCollection.Any())
            {
                try
                {
                    _entityLoader.Load(tripsCollection);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unable to load Green Taxi Data. Exception: {ex}");
                }
            }
        }

        #endregion Public Methods
    }
}