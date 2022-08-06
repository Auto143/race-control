using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Models
{
    internal class Meet
    {
        public Guid MeetID { get; set; }

        public string Name { get; set; } = String.Empty;

        public string TrackName { get; set; } = String.Empty;
        public Track? Track { get; set; }

        public string SeriesName { get; set;} = String.Empty;
        public Series? Series { get; set; }

        public DateTime StartDay { get; set; }

        public DateTime EndDay { get; set; }
    }
}
