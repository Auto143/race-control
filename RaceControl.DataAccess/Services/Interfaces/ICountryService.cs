using RaceControl.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface ICountryService
    {
        public bool CheckExists(string countryCode);

        public Country Get(string countryCode);

        public List<Country> GetAll();

        public List<Country> GetAllInContinent(string continentCode);

        public Country CreateNew(string countryCode);

        public void Update(Country country);

        public void Delete(string countryCode);
    }
}
