using Koerber.DB.Contracts;

namespace Koerber.DataLoader.Contracts;

public interface IEntityLoader<T> where T : IEntity
{
    #region Public Methods

    public void Load(IEnumerable<T> entityCollection);

    #endregion Public Methods
}