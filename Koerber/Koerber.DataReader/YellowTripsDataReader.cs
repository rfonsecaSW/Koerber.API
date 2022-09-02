using CsvHelper;
using CsvHelper.Configuration;
using Koerber.DataReader.Contracts;
using Koerber.DB.DataModels;
using System.Globalization;

namespace Koerber.DataReader;

public sealed class YellowTripsDataReader : IEntityReader<Trips>
{
    #region Public Methods

    public IEnumerable<Trips> Read(string filePath)
    {
        IList<Trips> yellowTrips;

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Yellow Trips File does not exist");
        }

        using (var reader = new StreamReader(filePath))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<YellowTripsMap>();
                yellowTrips = csv.GetRecords<Trips>().ToList();
            }
        }

        return yellowTrips;
    }

    #endregion Public Methods

    #region Private Classes

    private sealed class YellowTripsMap : ClassMap<Trips>
    {
        #region Public Constructors

        public YellowTripsMap()
        {
            Map(y => y.PickUpTime).Index(1);
            Map(y => y.DropOffTime).Index(2);
            Map(y => y.PickUpLocationID).Index(7);
            Map(y => y.DropOffLocationID).Index(8);
        }

        #endregion Public Constructors
    }

    #endregion Private Classes
}