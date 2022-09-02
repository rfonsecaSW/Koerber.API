using CsvHelper;
using CsvHelper.Configuration;
using Koerber.DataReader.Contracts;
using Koerber.DB.DataModels;
using System.Globalization;

namespace Koerber.DataReader;

public sealed class GreenTripsDataReader : IEntityReader<Trips>
{
    #region Public Methods

    public IEnumerable<Trips> Read(string filePath)
    {
        IList<Trips> greenTrips;

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Green Trips File does not exist");
        }

        using (var reader = new StreamReader(filePath))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GreenTripsMap>();
                greenTrips = csv.GetRecords<Trips>().ToList();
            }
        }

        return greenTrips;
    }

    #endregion Public Methods

    #region Private Classes

    private sealed class GreenTripsMap : ClassMap<Trips>
    {
        #region Public Constructors

        public GreenTripsMap()
        {
            Map(g => g.PickUpTime).Index(1);
            Map(g => g.DropOffTime).Index(2);
            Map(g => g.PickUpLocationID).Index(5);
            Map(g => g.DropOffLocationID).Index(6);
        }

        #endregion Public Constructors
    }

    #endregion Private Classes
}