using Koerber.DB.Contracts;

namespace Koerber.API.Contracts;

public interface IEntityServices<T> where T : IEntity
{
    #region Public Methods

    /// <summary>
    /// Synchronizes IEntity data storage with IEntity data sources
    /// </summary>
    public void Synchronize();

    #endregion Public Methods
}