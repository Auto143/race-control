﻿using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    public class DataService : IDataService
    {
        public IContinentService Continent { get; }

        public ICountryService Country { get; }

        public IMeetService Meet { get; }

        public ISeriesService Series { get; }

        private RCSQLiteContext _dataContext { get; }

        public DataService(string dbName, string appDBFolderStructure)
        {
            _dataContext = new RCSQLiteContext(dbName, appDBFolderStructure);

            _dataContext.Database.EnsureCreated();

            Continent = new ContinentService(_dataContext);

            Country = new CountryService(_dataContext);

            Series = new SeriesService(_dataContext);
        }

        public void DeleteSource()
        {
            _dataContext.Database.EnsureDeleted();
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}
