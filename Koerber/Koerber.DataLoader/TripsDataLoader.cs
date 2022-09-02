using Koerber.DataLoader.Contracts;
using Koerber.DB;
using Koerber.DB.DataModels;

namespace Koerber.DataLoader;

public static class LinqExtensions
{
    #region Public Methods

    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
    {
        using (var enumerator = source.GetEnumerator())
            while (enumerator.MoveNext())
                yield return YieldBatchElements(enumerator, batchSize - 1);
    }

    #endregion Public Methods

    #region Private Methods

    private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int batchSize)
    {
        yield return source.Current;
        for (int i = 0; i < batchSize && source.MoveNext(); i++)
            yield return source.Current;
    }

    #endregion Private Methods
}

public sealed class TripsDataLoader : IEntityLoader<Trips>
{
    #region Private Fields

    private readonly TaxiTripsContext _taxiTripsContext;

    #endregion Private Fields

    #region Public Constructors

    public TripsDataLoader(TaxiTripsContext taxiTripsContext)
    {
        _taxiTripsContext = taxiTripsContext;
    }

    #endregion Public Constructors

    #region Public Methods

    public void Load(IEnumerable<Trips> entityCollection)
    {
        using var transaction = _taxiTripsContext.Database.BeginTransaction();

        try
        {
            foreach (var batch in entityCollection.Batch(100000))
            {
                _taxiTripsContext.Trips.AddRange(batch);

                _taxiTripsContext.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            throw new Exception($"Unable to load Trips Entity Collection. Exception: {ex}");
        }

        transaction.Commit();
    }

    #endregion Public Methods
}