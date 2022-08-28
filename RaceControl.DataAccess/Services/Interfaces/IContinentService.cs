using RaceControl.DataAccess.Models;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface IContinentService
    {
        public bool CheckExists(string continentCode);

        public Continent Get(string continentCode);

        public List<Continent> GetAll();

        public Continent CreateNew(string continentCode);

        public void Update(Continent continent);

        public void Delete(string continentCode);
    }
}
