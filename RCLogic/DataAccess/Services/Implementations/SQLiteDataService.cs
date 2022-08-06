using Microsoft.EntityFrameworkCore;
using RCLogic.DataAccess.Context;
using RCLogic.DataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Services.Implementations
{
    public class SQLiteDataService : IDataService
    {
        private RCSQLiteContext _rcSQLiteContext { get; }

        public SQLiteDataService(string dbName, string appDBFolderStucture)
        {
            _rcSQLiteContext = new RCSQLiteContext(dbName, appDBFolderStucture);
        }

        public void DeleteSource()
        {
            _rcSQLiteContext.Database.EnsureDeleted();
        }

        public void Dispose()
        {
            _rcSQLiteContext.Dispose();
        }
    }
}
