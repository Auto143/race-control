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
    public class SQLiteDatabaseService : IDatabaseService
    {
        public DbContext DBContext { get; }
        
        public SQLiteDatabaseService(string dbName, string appDBFolderStucture)
        {
            DBContext = new RCSQLiteContext(dbName, appDBFolderStucture);
        }

        public void Dispose()
        {
            DBContext.Dispose();
        }
    }
}
