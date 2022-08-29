using RaceControl.DataAccess.Models;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface IRaceMeetService
    {
        public bool CheckExists(Guid raceMeetID);

        public RaceMeet Get(Guid raceMeetID);

        public List<RaceMeet> GetAll();

        public List<RaceMeet> GetAllAtTrack(string trackName);

        public RaceMeet CreateNew(string trackName, string seriesName);

        public void Update(RaceMeet raceMeet);

        public void Delete(Guid raceMeetID);
    }
}
