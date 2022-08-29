
namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface IDataService
    {
        public IContinentService Continent { get; }

        public ICountryService Country { get; }

        public IRaceMeetService RaceMeet { get; }

        public ISeriesService Series { get; }
        
        public ITrackService Track { get; }

        public void DeleteSource();

        public void Dispose();
    }
}
