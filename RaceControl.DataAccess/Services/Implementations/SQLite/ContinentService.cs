using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    internal class ContinentService : IContinentService
    {
        private readonly RCSQLiteContext _dataContext;

        internal ContinentService(RCSQLiteContext rcSQLiteContext)
        {
            _dataContext = rcSQLiteContext;
        }

        public bool CheckExists(string continentCode)
        {
            if (_dataContext.Continents.FirstOrDefault(c => c.ContinentCode == continentCode) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Continent Get(string continentCode)
        {
            try
            {
                return _dataContext.Continents.First(c => c.ContinentCode == continentCode);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Equals("sequence contains no elements"))
                {
                    throw new KeyNotFoundException($"No continent exists with code {continentCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public List<Continent> GetAll()
        {
            return _dataContext.Continents.ToList();
        }

        public Continent CreateNew(string continentCode)
        {
            try
            {
                var newContinent = new Continent
                {
                    ContinentCode = continentCode
                };

                _dataContext.Continents.Add(newContinent);
                _dataContext.SaveChanges();

                return newContinent;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Contains("cannot be tracked because another instance with the same key value"))
                {
                    throw new ArgumentException($"Continent already exists with code {continentCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Update(Continent continent)
        {
            try
            {
                _dataContext.Continents.Update(continent);
                _dataContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                if (dbUpdateConcurrencyException.Message.ToLower().Contains("the database operation was expected to affect"))
                {
                    throw new KeyNotFoundException($"No continent exists with code {continent.ContinentCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Delete(string continentCode)
        {
            Continent continentToRemove = Get(continentCode);

            _dataContext.Continents.Remove(continentToRemove);
            _dataContext.SaveChanges();
        }
    }
}
