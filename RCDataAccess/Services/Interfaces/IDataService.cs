using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCDataAccess.Services.Interfaces
{
    public interface IDataService
    {
        public IMeetService Meet { get; }

        public ISeriesService Series { get; }

        public void DeleteSource();

        public void Dispose();
    }
}
