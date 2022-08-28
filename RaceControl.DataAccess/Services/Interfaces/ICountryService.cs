using RaceControl.DataAccess.Models;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface ICountryService
    {
        public bool CheckExists(string countryCode);

        public Country Get(string countryCode);

        public List<Country> GetAll();

        public List<Country> GetAllInContinent(string continentCode);

        public Country CreateNew(string countryCode, string continentCode);

        public void Update(Country country);

        public void Delete(string countryCode);
    }
}
