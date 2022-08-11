using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    public class DataService : IDataService
    {
        public IMeetService Meet { get; }

        public ISeriesService Series { get; }

        internal RCSQLiteContext rcSQLiteContext { get; }

        public DataService(string dbName, string appDBFolderStucture)
        {
            rcSQLiteContext = new RCSQLiteContext(dbName, appDBFolderStucture);

            rcSQLiteContext.Database.EnsureCreated();

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
