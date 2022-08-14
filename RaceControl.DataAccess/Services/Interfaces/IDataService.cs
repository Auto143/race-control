using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface IDataService
    {
        public IContinentService Continent { get; }

        public IMeetService Meet { get; }

        public ISeriesService Series { get; }

        public void DeleteSource();

        public void Dispose();
    }
}
