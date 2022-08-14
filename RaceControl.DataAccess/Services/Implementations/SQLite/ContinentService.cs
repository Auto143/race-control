using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    internal class ContinentService : IContinentService
    {
        private RCSQLiteContext _dataContext;

        internal ContinentService(RCSQLiteContext rcSQLiteContext)
        {
            _dataContext = rcSQLiteContext;
        }

        public bool CheckExists(string continentCode)
        {
            if (_dataContext.Continents.Where(c => c.ContinentCode == continentCode).FirstOrDefault() == null)
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
                return _dataContext.Continents.Where(c => c.ContinentCode == continentCode).First();
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Equals("sequence contains no elements"))
                {
                    throw new KeyNotFoundException(String.Format("No continent exists with code {0}", continentCode));
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
                Continent newContinent = new Continent();
                newContinent.ContinentCode = continentCode;

                _dataContext.Continents.Add(newContinent);
                _dataContext.SaveChanges();

                return newContinent;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Contains("cannot be tracked because another instance with the same key value"))
                {
                    throw new ArgumentException(String.Format("Continent already exists with code {0}", continentCode));
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
                    throw new KeyNotFoundException(String.Format("No continent exists with code {0}", continent.ContinentCode));
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
