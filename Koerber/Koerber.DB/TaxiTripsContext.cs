using Koerber.DB.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Koerber.DB;

public sealed class TaxiTripsContext : DbContext
{
    #region Public Constructors

    public TaxiTripsContext(DbContextOptions<TaxiTripsContext> options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    #endregion Public Constructors

    #region Public Properties

    public DbSet<Trips> Trips { get; set; }
    public DbSet<Zones> Zones { get; set; }

    #endregion Public Properties

    #region Protected Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trips>()
                .HasOne(m => m.PickUpZone)
                .WithMany(t => t.PickUpTrips)
                .HasForeignKey(m => m.PickUpLocationID)
                .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Trips>()
                .HasOne(m => m.DropOffZone)
                .WithMany(t => t.DropOffTrips)
                .HasForeignKey(m => m.DropOffLocationID)
                .OnDelete(DeleteBehavior.NoAction);
    }

    #endregion Protected Methods
}