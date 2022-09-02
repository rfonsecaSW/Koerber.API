using CsvHelper;
using CsvHelper.Configuration;
using Koerber.DataReader.Contracts;
using Koerber.DB.DataModels;
using System.Globalization;

namespace Koerber.DataReader;

public sealed class ZonesDataReader : IEntityReader<Zones>
{
    #region Public Methods

    public IEnumerable<Zones> Read(string filePath)
    {
        IList<Zones> zones;

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Zones File does not exist");
        }

        using (var reader = new StreamReader(filePath))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<ZonesMap>();
                zones = csv.GetRecords<Zones>().ToList();
            }
        }

        return zones;
    }

    #endregion Public Methods

    #region Private Classes

    private sealed class ZonesMap : ClassMap<Zones>
    {
        #region Public Constructors

        public ZonesMap()
        {
            Map(z => z.LocationID).Index(0);
            Map(z => z.Borough).Index(1);
            Map(z => z.Zone).Index(2);
            Map(z => z.ServiceZone).Index(3);
        }

        #endregion Public Constructors
    }

    #endregion Private Classes
}