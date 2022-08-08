﻿using Microsoft.EntityFrameworkCore;
using RCDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCDataAccess.Contexts
{
    internal class RCSQLiteContext : DbContext
    {
        public DbSet<Continent> Continents => Set<Continent>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Meet> Meets => Set<Meet>();
        public DbSet<Series> Series => Set<Series>();
        public DbSet<Track> Tracks => Set<Track>();

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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            configureContinentsTable(modelBuilder);

            configureContriesTable(modelBuilder);

            configureMeetsTable(modelBuilder);

            configureSeriesTable(modelBuilder);

            configureTracksTable(modelBuilder);
        }

        private void configureContinentsTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Continent>().HasKey(c => c.ContinentCode);
        }

        private void configureContriesTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasKey(c => c.CountryCode);

            modelBuilder.Entity<Country>()
                .HasOne(c => c.Continant)
                .WithMany()
                .HasForeignKey(c => c.ContinentCode);
        }

        private void configureMeetsTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meet>().HasKey(m => m.MeetID);

            modelBuilder.Entity<Meet>()
                .HasOne(m => m.Track)
                .WithMany()
                .HasForeignKey(m => m.TrackName);
            
            modelBuilder.Entity<Meet>()
                .HasOne(m => m.Series)
                .WithMany()
                .HasForeignKey(m => m.SeriesName);
        }

        private void configureSeriesTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Series>().HasKey(s => s.SeriesName);
        }

        private void configureTracksTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Track>().HasKey(t => t.TrackName);

            modelBuilder.Entity<Track>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryCode);
        }
    }
}