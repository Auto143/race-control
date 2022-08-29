using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.IntegrationTests.Services.SQLite
{
    [TestFixture]
    public class TrackServiceTests
    {
        [Test]
        public void CheckExistsCalled_IfTrackWithNameFound_ReturnTrue()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

            try
            {
                // Act
                bool doesExist = dataService.Track.CheckExists(TEST_TRACK_NAME);

                // Assert
                Assert.That(doesExist, Is.True);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoTrackWithNameFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                bool doesExist = dataService.Track.CheckExists(String.Empty);

                // Assert
                Assert.That(doesExist, Is.False);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void GetCalled_IfTrackWithNameFound_ReturnTrackObjectFromDatabase()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            Track testTrack = dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

            try
            {
                // Act
                Track returnedTrack = dataService.Track.Get(TEST_TRACK_NAME);

                // Assert
                Assert.That(testTrack, Is.EqualTo(returnedTrack));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoTrackWithNameFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void GetDelegate() => dataService.Track.Get(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(GetDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void GetAllCalled_IfAnyTracksExist_ReturnAllTrackObjectsFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE_ONE = "TestCountryCodeOne";
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";
            const string TEST_TRACK_NAME_ONE = "TestTrackOne";

            const string TEST_COUNTRY_CODE_TWO = "TestCountryCodeTwo";
            const string TEST_CONTINENT_CODE_TWO = "TestContinentCodeTwo";
            const string TEST_TRACK_NAME_TWO = "TestTrackTwo";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE_ONE, TEST_CONTINENT_CODE_ONE);
            Track testTrackOne = dataService.Track.CreateNew(TEST_TRACK_NAME_ONE, TEST_COUNTRY_CODE_ONE);

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE_TWO, TEST_CONTINENT_CODE_TWO);
            Track testTrackTwo = dataService.Track.CreateNew(TEST_TRACK_NAME_TWO, TEST_COUNTRY_CODE_TWO);

            try
            {
                // Act
                List<Track> returnedTracks = dataService.Track.GetAll();
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedTracks[0], Is.EqualTo(testTrackOne));
                    Assert.That(returnedTracks[1], Is.EqualTo(testTrackTwo));
                    Assert.That(returnedTracks, Has.Count.EqualTo(2));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoTracksExist_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<Track> returnedTracks = dataService.Track.GetAll();

                // Assert
                Assert.That(returnedTracks, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
       
        [Test]
        public void GetAllInCountryCalled_IfAnyTracksInCountryExist_ReturnAllTrackObjectsInCountryFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE_ONE = "TestCountryCodeOne";
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";
            const string TEST_TRACK_NAME_ONE = "TestTrackOne";

            const string TEST_COUNTRY_CODE_TWO = "TestCountryCodeTwo";
            const string TEST_CONTINENT_CODE_TWO = "TestContinentCodeTwo";
            const string TEST_TRACK_NAME_TWO = "TestTrackTwo";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE_ONE, TEST_CONTINENT_CODE_ONE);
            Track testTrackOne = dataService.Track.CreateNew(TEST_TRACK_NAME_ONE, TEST_COUNTRY_CODE_ONE);

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE_TWO, TEST_CONTINENT_CODE_TWO);
            dataService.Track.CreateNew(TEST_TRACK_NAME_TWO, TEST_COUNTRY_CODE_TWO);

            try
            {
                // Act
                List<Track> returnedTracks = dataService.Track.GetAllInCountry(TEST_COUNTRY_CODE_ONE);
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedTracks[0], Is.EqualTo(testTrackOne));
                    Assert.That(returnedTracks, Has.Count.EqualTo(1));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllInCountryCalled_IfNoTracksInCountryExist_ReturnEmptyList()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<Track> returnedTracks = dataService.Track.GetAllInCountry(TEST_COUNTRY_CODE);

                // Assert
                Assert.That(returnedTracks, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void CreateNewCalled_IfNoTrackWithNameExistsAndCountryWithCodeExists_SaveToDatabaseAndReturnTrackObject()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            try
            {
                // Act
                Track track = dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

                // Assert
                bool doesTestTrackExist = dataService.Track.CheckExists(TEST_TRACK_NAME);
                
                Assert.Multiple(() =>
                {
                    Assert.That(doesTestTrackExist, Is.True);
                    Assert.That(track.TrackName, Is.EqualTo(TEST_TRACK_NAME));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoTrackWithNameExistsAndCountryWithCodeDoesntExist_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "TestCountryCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void CreateNewDelegate() => dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

                // Assert
                Assert.Throws<KeyNotFoundException>(CreateNewDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfTrackWithNameAlreadyExists_ThrowArgumentException()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);
            
            try
            {
                // Act
                void CreateNewDelegate() => dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

                // Assert
                Assert.Throws<ArgumentException>(CreateNewDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void UpdateCalled_WhenTrackWithNameAlreadyExists_UpdateTrackInDatabase()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const double TEST_TRACK_LENGTH = 0.0;
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            Track track = dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

            track.Length = TEST_TRACK_LENGTH;

            try
            {
                // Act
                dataService.Track.Update(track);

                // Assert
                Track returnedTrack = dataService.Track.Get(TEST_TRACK_NAME);

                Assert.That(track.Length, Is.EqualTo(returnedTrack.Length));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenNoTrackWithNameExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const double TEST_TRACK_LENGTH = 0.0;
            const string TEST_COUNTRY_CODE = "CountryCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            var track = new Track
            {
                TrackName = TEST_TRACK_NAME,
                CountryCode = TEST_COUNTRY_CODE,
                Length = TEST_TRACK_LENGTH
            };

            try
            {
                // Act
                void UpdateDelegate() => dataService.Track.Update(track);

                // Assert
                Assert.Throws<KeyNotFoundException>(UpdateDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void DeleteCalled_WhenTrackWithNameFound_RemoveTrackFromDatabase()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

            bool doesExistBefore = dataService.Country.CheckExists(TEST_COUNTRY_CODE);

            try
            {
                // Act
                dataService.Country.Delete(TEST_COUNTRY_CODE);

                // Assert
                bool doesExistAfter = dataService.Country.CheckExists(TEST_COUNTRY_CODE);
                
                Assert.Multiple(() =>
                {
                    Assert.That(doesExistBefore, Is.True);
                    Assert.That(doesExistAfter, Is.False);
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenNoTrackWithNameFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void DeleteDelegate() => dataService.Track.Delete(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(DeleteDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        private static (IDataService, string) CreateDataService()
        {
            const string TEST_DB_NAME = "TestDatabase";

            string testFolderStructure = $"RCDataAccessTests{Guid.NewGuid()}";
            string testFolderPath = GetTestDbFolderPath(testFolderStructure);

            return (new DataService(TEST_DB_NAME, testFolderStructure), testFolderPath);
        }

        private static void CleanUpDataService(IDataService? dataService, string testFolderPath)
        {
            if (dataService != null)
            {
                dataService.DeleteSource();
                dataService.Dispose();
                Directory.Delete(testFolderPath, true);
            }
        }

        private static string GetTestDbFolderPath(string folderStructure)
        {
            Environment.SpecialFolder appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string appDataFolderPath = Environment.GetFolderPath(appDataFolder);

            string testDatabaseFolderPath = Path.Join(appDataFolderPath, folderStructure);

            return testDatabaseFolderPath;
        }
    }
}