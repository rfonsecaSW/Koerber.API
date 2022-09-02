using Koerber.DB.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Koerber.DB.DataModels;

public sealed class Zones : IEntity
{
    #region Public Properties

    [Required]
    [MaxLength(50)]
    public string? Borough { get; set; }

    [Required]
    public List<Trips>? DropOffTrips { get; set; }

    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int LocationID { get; set; }

    [Required]
    public List<Trips>? PickUpTrips { get; set; }

    [Required]
    [MaxLength(50)]
    public string? ServiceZone { get; set; }

    [Required]
    [MaxLength(50)]
    public string? Zone { get; set; }

    #endregion Public Properties
}