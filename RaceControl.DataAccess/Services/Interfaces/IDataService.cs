
namespace RaceControl.DataAccess.Services.Interfaces
{
    public interface IDataService
    {
        public IContinentService Continent { get; }

        public ICountryService Country { get; }

        public IMeetService Meet { get; }

        public ISeriesService Series { get; }

        public void DeleteSource();

        public void Dispose();
    }
}
