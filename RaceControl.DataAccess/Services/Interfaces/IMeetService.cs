using RaceControl.DataAccess.Models;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface IMeetService
    {
        public bool CheckExists(Guid meetID);

        public Meet Get(Guid meetID);

        public List<Meet> GetAll();

        public List<Meet> GetAllAtTrack(string trackName);

        public Meet CreateNew();

        public void Update(Meet meet);

        public void Delete(Guid meetID);
    }
}
