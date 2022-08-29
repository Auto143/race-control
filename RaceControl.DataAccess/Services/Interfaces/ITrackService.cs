using RaceControl.DataAccess.Models;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface ITrackService
    {
        public bool CheckExists(string trackName);

        public Track Get(string trackName);

        public List<Track> GetAll();

        public List<Track> GetAllInCountry(string countryCode);

        public Track CreateNew(string trackName, string countryCode);

        public void Update(Track track);

        public void Delete(string trackName);
    }
}