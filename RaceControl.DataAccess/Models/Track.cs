
namespace RaceControl.DataAccess.Models
{
    public class Track
    {
        public string TrackName { get; set; } = String.Empty;

        public double Length { get; set; }

        public string CountryCode { get; set; } = String.Empty;
        public Country? Country { get; set; }
    }
}
