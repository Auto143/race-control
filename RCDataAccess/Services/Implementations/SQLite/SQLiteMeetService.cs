using RCDataAccess.Contexts;
using RCDataAccess.Models;
using RCDataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCDataAccess.Services.Implementations.SQLite
{
    internal class SQLiteMeetService : IMeetService
    {
        private RCSQLiteContext _dataContext;

        internal SQLiteMeetService(RCSQLiteContext rcSQLiteContext)
        {
            _dataContext = rcSQLiteContext;
        }

        public bool CheckExists(Guid meetID)
        {
            throw new NotImplementedException();
        }

        public Meet Get(Guid meetID)
        {
            try
            {
                return _dataContext.Meets.Where(m => m.MeetID == meetID).First();
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message.ToLower().Equals("sequence contains no elements"))
                {
                    throw new KeyNotFoundException(String.Format("No meet exists with key {0}"));
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public List<Meet> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Meet> GetAllAtTrack(string trackName)
        {
            throw new NotImplementedException();
        }

        public Meet CreateNew()
        {
            throw new NotImplementedException();
        }

        public void Update(Meet meet)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid MeetID)
        {
            throw new NotImplementedException();
        }
    }
}
