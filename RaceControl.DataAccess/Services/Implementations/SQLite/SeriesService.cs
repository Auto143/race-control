using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    internal class SeriesService : ISeriesService
    {
        private readonly RCSQLiteContext _dataContext;

        internal SeriesService(RCSQLiteContext rcSQLiteContext)
        {
            _dataContext = rcSQLiteContext;
        }

        public bool CheckExists(string seriesName)
        {
            if (_dataContext.Series.FirstOrDefault(s => s.SeriesName == seriesName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Series Get(string seriesName)
        {
            try
            {
                return _dataContext.Series.First(s => s.SeriesName == seriesName);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Equals("sequence contains no elements"))
                {
                    throw new KeyNotFoundException($"No series exists with key {seriesName}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public List<Series> GetAll()
        {
            return _dataContext.Series.ToList();
        }

        public Series CreateNew(string seriesName)
        {
            try
            {
                var newSeries = new Series
                {
                    SeriesName = seriesName
                };

                _dataContext.Series.Add(newSeries);
                _dataContext.SaveChanges();

                return newSeries;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Contains("cannot be tracked because another instance with the same key value"))
                {
                    throw new ArgumentException($"Series already exists with key {seriesName}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Update(Series series)
        {
            try
            { 
                _dataContext.Series.Update(series);
                _dataContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                if (dbUpdateConcurrencyException.Message.ToLower().Contains("the database operation was expected to affect"))
                {
                    throw new KeyNotFoundException($"No series exists with key {series.SeriesName}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Delete(string seriesName)
        {
            Series seriesToRemove = Get(seriesName);

            _dataContext.Series.Remove(seriesToRemove);
            _dataContext.SaveChanges();
        }
    }
}
