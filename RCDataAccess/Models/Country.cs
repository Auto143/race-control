using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCDataAccess.Models
{
    public class Country
    {
        public string CountryCode { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string ContinentCode { get; set; } = String.Empty;
        public Continent? Continant { get; set; }
    }
}
