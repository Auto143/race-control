using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Models;

namespace RaceControl.DataAccess.Contexts
{
    internal class RCSQLiteContext : DbContext
    {
        public DbSet<Continent> Continents => Set<Continent>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<RaceMeet> RaceMeets => Set<RaceMeet>();
        public DbSet<Series> Series => Set<Series>();
        public DbSet<Track> Tracks => Set<Track>();

        private readonly string _dbPath; 

        internal RCSQLiteContext(string dbName, string appDBFolderStructure)
        {
            Environment.SpecialFolder appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string appDataFolderPath = Environment.GetFolderPath(appDataFolder);

            string dbFolderPath = Path.Join(appDataFolderPath, appDBFolderStructure);

            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
            }

            _dbPath = Path.Join(dbFolderPath, $"{dbName}.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_dbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureContinentsTable(modelBuilder);

            ConfigureCountriesTable(modelBuilder);

            ConfigureMeetsTable(modelBuilder);

            ConfigureSeriesTable(modelBuilder);

            ConfigureTracksTable(modelBuilder);
        }

        private static void ConfigureContinentsTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Continent>().HasKey(c => c.ContinentCode);
        }

        private static void ConfigureCountriesTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasKey(c => c.CountryCode);

            modelBuilder.Entity<Country>()
                .HasOne(c => c.Continent)
                .WithMany()
                .HasForeignKey(c => c.ContinentCode);
        }

        private static void ConfigureMeetsTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RaceMeet>().HasKey(m => m.RaceMeetID);

            modelBuilder.Entity<RaceMeet>()
                .HasOne(m => m.Track)
                .WithMany()
                .HasForeignKey(m => m.TrackName);
            
            modelBuilder.Entity<RaceMeet>()
                .HasOne(m => m.Series)
                .WithMany()
                .HasForeignKey(m => m.SeriesName);
        }

        private static void ConfigureSeriesTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Series>().HasKey(s => s.SeriesName);
        }

        private static void ConfigureTracksTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Track>().HasKey(t => t.TrackName);

            modelBuilder.Entity<Track>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryCode);
        }
    }
}
