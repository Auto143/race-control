using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Models
{
    internal class Track
    {
        public string TrackName { get; set; } = String.Empty;

        public double Length { get; set; }

        public string CountryCode { get; set; } = String.Empty;
        public Country? Country { get; set; }
    }
}
