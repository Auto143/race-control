using RaceControl.DataAccess.Models;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface ISeriesService
    {
        public bool CheckExists(string seriesName);

        public Series Get(string seriesName);

        public List<Series> GetAll();

        public Series CreateNew(string seriesName);

        public void Update(Series series);

        public void Delete(string seriesName);
    }
}
