using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.Models
{
    public class RaceMeet
    {
        public Guid RaceMeetID { get; set; }

        public string Description { get; set; } = String.Empty;

        public string TrackName { get; set; } = String.Empty;
        public Track? Track { get; set; }

        public string SeriesName { get; set;} = String.Empty;
        public Series? Series { get; set; }

        public DateTime StartDay { get; set; }

        public DateTime EndDay { get; set; }
    }
}
