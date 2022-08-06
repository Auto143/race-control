﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Context
{
    internal class RCSQLiteContext : DbContext
    {
        private string _dbPath;

        internal RCSQLiteContext(string dbName, string appDBFolderStucture)
        {
            Environment.SpecialFolder appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string appDataFolderPath = Environment.GetFolderPath(appDataFolder);

            string dbFolderPath = Path.Join(appDataFolderPath, appDBFolderStucture);

            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
            }

            _dbPath = Path.Join(dbFolderPath, String.Format("{0}.db", dbName));

            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dbPath}");
    }
}