using Koerber.API.Models;

namespace Koerber.UnitTests.Fixtures;

internal sealed class TopZonesFixtures
{
    #region Public Methods

    public static TopZonesOutput GetTestTopZones()
    {
        return new TopZonesOutput()
        {
            TopZones = new List<TopZone>()
            {
                new TopZone
                {
                    Zone = "MidTown East",
                    DropOffTotal = 435,
                    PickUpTotal = 321
                },
                new TopZone
                {
                    Zone = "Jackson Heights",
                    DropOffTotal = 324,
                    PickUpTotal = 456
                },
                new TopZone
                {
                    Zone = "Queens",
                    DropOffTotal = 124,
                    PickUpTotal = 98
                },
                new TopZone
                {
                    Zone = "Bronx",
                    DropOffTotal = 54,
                    PickUpTotal = 98
                },
                new TopZone
                {
                    Zone = "Manhattan",
                    DropOffTotal = 543,
                    PickUpTotal = 24
                }
            }
        };
    }

    #endregion Public Methods
}