using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    public class DataService : IDataService
    {
        public IContinentService Continent { get; }

        public ICountryService Country { get; }

        public IMeetService Meet { get; }

        public ISeriesService Series { get; }

        internal RCSQLiteContext rcSQLiteContext { get; }

        public DataService(string dbName, string appDBFolderStucture)
        {
            rcSQLiteContext = new RCSQLiteContext(dbName, appDBFolderStucture);

            rcSQLiteContext.Database.EnsureCreated();

            Continent = new ContinentService(rcSQLiteContext);

            Country = new CountryService(rcSQLiteContext);

            Series = new SeriesService(rcSQLiteContext);
        }

        public void DeleteSource()
        {
            rcSQLiteContext.Database.EnsureDeleted();
        }

        public void Dispose()
        {
            rcSQLiteContext.Dispose();
        }
    }
}
