using Koerber.DB.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Koerber.DB.DataModels;

public sealed class Trips : IEntity
{
    #region Public Properties

    [Required]
    [ForeignKey(nameof(DropOffZone))]
    public int DropOffLocationID { get; set; }

    [Required]
    public DateTime DropOffTime { get; set; }

    public Zones? DropOffZone { get; set; }

    [Required]
    [ForeignKey(nameof(PickUpZone))]
    public int PickUpLocationID { get; set; }

    [Required]
    public DateTime PickUpTime { get; set; }

    public Zones? PickUpZone { get; set; }

    [Required]
    [Key]
    public int TripId { get; set; }

    #endregion Public Properties
}