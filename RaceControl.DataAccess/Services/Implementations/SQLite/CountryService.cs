using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    internal class CountryService : ICountryService
    {
        private readonly RCSQLiteContext _dataContext;

        internal CountryService(RCSQLiteContext rcSQLiteContext)
        {
            _dataContext = rcSQLiteContext;
        }

        public bool CheckExists(string countryCode)
        {
            if (_dataContext.Countries.FirstOrDefault(c => c.CountryCode == countryCode) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Country Get(string countryCode)
        {
            try
            {
                return _dataContext.Countries.First(c => c.CountryCode == countryCode);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Equals("sequence contains no elements"))
                {
                    throw new KeyNotFoundException($"No country exists with code {countryCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public List<Country> GetAll()
        {
            return _dataContext.Countries.ToList();
        }

        public List<Country> GetAllInContinent(string continentCode)
        {
            return _dataContext.Countries.Where(c => c.ContinentCode == continentCode).ToList();
        }

        public Country CreateNew(string countryCode, string continentCode)
        {
            try
            {
                var newCountry = new Country
                {
                    CountryCode = countryCode,
                    ContinentCode = continentCode
                };

                _dataContext.Countries.Add(newCountry);
                _dataContext.SaveChanges();

                return newCountry;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Contains("cannot be tracked because another instance with the same key value"))
                {
                    throw new ArgumentException($"Country already exists with code {countryCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.Message.ToLower().Contains("an error occurred while saving the entity changes"))
                {
                    throw new KeyNotFoundException($"No continent exists with code {continentCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Update(Country country)
        {
            try
            {
                _dataContext.Countries.Update(country);
                _dataContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                if (dbUpdateConcurrencyException.Message.ToLower().Contains("the database operation was expected to affect"))
                {
                    throw new KeyNotFoundException($"No country exists with code {country.CountryCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Delete(string countryCode)
        {
            Country countryToRemove = Get(countryCode);

            _dataContext.Countries.Remove(countryToRemove);
            _dataContext.SaveChanges();
        }
    }
}
