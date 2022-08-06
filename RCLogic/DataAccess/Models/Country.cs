using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogic.DataAccess.Models
{
    internal class Country
    {
        public string CountryCode { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string ContinentCode { get; set; } = String.Empty;

        public Continent? Continant { get; set; }
    }
}
