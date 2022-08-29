using Microsoft.EntityFrameworkCore;
using RaceControl.DataAccess.Contexts;
using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.Services.Implementations.SQLite
{
    public class TrackService : ITrackService
    {
        private readonly RCSQLiteContext _dataContext;

        internal TrackService(RCSQLiteContext rcSQLiteContext)
        {
            _dataContext = rcSQLiteContext;
        }
        
        public bool CheckExists(string trackName)
        {
            if (_dataContext.Tracks.FirstOrDefault(t => t.TrackName == trackName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Track Get(string trackName)
        {
            try
            {
                return _dataContext.Tracks.First(t => t.TrackName == trackName);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Equals("sequence contains no elements"))
                {
                    throw new KeyNotFoundException($"No track exists with name {trackName}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public List<Track> GetAll()
        {
            return _dataContext.Tracks.ToList();
        }

        public List<Track> GetAllInCountry(string countryCode)
        {
            return _dataContext.Tracks.Where(t => t.CountryCode == countryCode).ToList();
        }

        public Track CreateNew(string trackName, string countryCode)
        {
            try
            {
                var newTrack = new Track
                {
                    TrackName = trackName,
                    CountryCode = countryCode
                };

                _dataContext.Tracks.Add(newTrack);
                _dataContext.SaveChanges();

                return newTrack;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Contains("cannot be tracked because another instance with the same key value"))
                {
                    throw new ArgumentException($"Track already exists with name {trackName}");
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
                    throw new KeyNotFoundException($"No country exists with code {countryCode}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Update(Track track)
        {
            try
            {
                _dataContext.Tracks.Update(track);
                _dataContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                if (dbUpdateConcurrencyException.Message.ToLower().Contains("the database operation was expected to affect"))
                {
                    throw new KeyNotFoundException($"No track exists with name {track.TrackName}");
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Delete(string trackName)
        {
            Track trackToRemove = Get(trackName);

            _dataContext.Tracks.Remove(trackToRemove);
            _dataContext.SaveChanges();
        }
    }
}