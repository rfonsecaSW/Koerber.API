using Koerber.DB.Contracts;

namespace Koerber.DataReader.Contracts;

public interface IEntityReader<T> where T : IEntity
{
    #region Public Methods

    public IEnumerable<T> Read(string filePath);

    #endregion Public Methods
}