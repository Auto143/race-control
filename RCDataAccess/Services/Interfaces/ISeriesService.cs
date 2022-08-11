using RaceControl.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface ISeriesService
    {
        public bool CheckExists(string seriesName);

        public Series Get(string seriesName);

        public List<Series> GetAll();

        public Series CreateNew(string seriesName);

        public void Update(Series series);

        public void Delete(string seriesName);
    }
}
