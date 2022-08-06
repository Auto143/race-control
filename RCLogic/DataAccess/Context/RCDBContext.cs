using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Context
{
    internal class RCDBContext : DbContext
    {
        private const string _appDBFolderStructure = "/RaceControl/Database/";
        private const string _dbName = "racecontrol";
        private string _dbPath;

        internal RCDBContext()
        {
            Environment.SpecialFolder appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string appDataFolderPath = Environment.GetFolderPath(appDataFolder);

            string dbFolderPath = Path.Join(appDataFolderPath, _appDBFolderStructure);

            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
            }

            _dbPath = Path.Join(dbFolderPath, _dbName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dbPath}");
    }
}
