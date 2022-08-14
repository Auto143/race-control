using RaceControl.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
