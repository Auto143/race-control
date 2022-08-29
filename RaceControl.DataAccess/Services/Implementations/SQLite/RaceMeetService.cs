using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    public class RaceMeetService : IRaceMeetService
    {
        private readonly RCSQLiteContext _dataContext;

        internal RaceMeetService(RCSQLiteContext rcSQLiteContext)
        {
            _dataContext = rcSQLiteContext;
        }

        public bool CheckExists(Guid raceMeetID)
        {
            if (_dataContext.RaceMeets.FirstOrDefault(rm => rm.RaceMeetID == raceMeetID) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public RaceMeet Get(Guid raceMeetID)
        {
            try
            {
                return _dataContext.RaceMeets.First(rm => rm.RaceMeetID == raceMeetID);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Equals("sequence contains no elements"))
                {
                    throw new KeyNotFoundException($"No meet exists with ID {raceMeetID}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public List<RaceMeet> GetAll()
        {
            return _dataContext.RaceMeets.ToList();
        }

        public List<RaceMeet> GetAllAtTrack(string trackName)
        {
            return _dataContext.RaceMeets.Where(rm => rm.TrackName == trackName).ToList();
        }

        public RaceMeet CreateNew(string trackName, string seriesName)
        {
            try
            {
                var newRaceMeet = new RaceMeet
                {
                    RaceMeetID = Guid.NewGuid(),
                    TrackName = trackName,
                    SeriesName = seriesName
                };

                _dataContext.RaceMeets.Add(newRaceMeet);
                _dataContext.SaveChanges();

                return newRaceMeet;
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.Message.ToLower().Contains("an error occurred while saving the entity changes"))
                {
                    throw new KeyNotFoundException("Track or Series doesn't exist");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Update(RaceMeet raceMeet)
        {
            try
            {
                if (CheckExists(raceMeet.RaceMeetID) == true)
                {
                    _dataContext.RaceMeets.Update(raceMeet);
                    _dataContext.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"No race meet exists with ID {raceMeet.RaceMeetID}");
                }
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                if (dbUpdateConcurrencyException.Message.ToLower().Contains("the database operation was expected to affect"))
                {
                    throw new KeyNotFoundException($"No race meet exists with ID {raceMeet.RaceMeetID}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Delete(Guid raceMeetID)
        {
            RaceMeet raceMeetToRemove = Get(raceMeetID);

            _dataContext.RaceMeets.Remove(raceMeetToRemove);
            _dataContext.SaveChanges();
        }
    }
}