
namespace RaceControl.DataAccess.Models
{
    public class Country
    {
        public string CountryCode { get; set; } = String.Empty;

        public string Name { get; set; } = String.Empty;

        public string ContinentCode { get; set; } = String.Empty;
        public Continent? Continent { get; set; }
    }
}
