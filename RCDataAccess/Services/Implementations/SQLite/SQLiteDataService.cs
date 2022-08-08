﻿using Microsoft.EntityFrameworkCore;
using RCDataAccess.Contexts;
using RCDataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCDataAccess.Services.Implementations.SQLite
{
    public class SQLiteDataService : IDataService
    {
        internal RCSQLiteContext rcSQLiteContext { get; }

        public SQLiteDataService(string dbName, string appDBFolderStucture)
        {
            rcSQLiteContext = new RCSQLiteContext(dbName, appDBFolderStucture);
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