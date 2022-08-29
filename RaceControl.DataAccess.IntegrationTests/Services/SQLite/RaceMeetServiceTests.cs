using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.IntegrationTests.Services.SQLite
{
    [TestFixture]
    public class RaceMeetServiceTests
    {
        [Test]
        public void CheckExistsCalled_IfRaceMeetWithIDFound_ReturnTrue()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);
            dataService.Series.CreateNew(TEST_SERIES_NAME);
            RaceMeet raceMeet = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);

            try
            {
                // Act
                bool doesExist = dataService.RaceMeet.CheckExists(raceMeet.RaceMeetID);

                // Assert
                Assert.That(doesExist, Is.True);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoRaceMeetWithIDFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                bool doesExist = dataService.RaceMeet.CheckExists(Guid.Empty);

                // Assert
                Assert.That(doesExist, Is.False);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void GetCalled_IfRaceMeetWithIDFound_ReturnRaceMeetObjectFromDatabase()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);
            dataService.Series.CreateNew(TEST_SERIES_NAME);
            
            RaceMeet testRaceMeet = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);
            Guid testRaceMeetID = testRaceMeet.RaceMeetID;
            
            try
            {
                // Act
                RaceMeet returnedRaceMeet = dataService.RaceMeet.Get(testRaceMeetID);

                // Assert
                Assert.That(testRaceMeet, Is.EqualTo(returnedRaceMeet));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoRaceMeetWithIDFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void GetDelegate() => dataService.RaceMeet.Get(Guid.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(GetDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void GetAllCalled_IfAnyRaceMeetsExist_ReturnAllRaceMeetObjectsFromDatabase()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);
            dataService.Series.CreateNew(TEST_SERIES_NAME);
            
            RaceMeet testRaceMeetOne = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);
            RaceMeet testRaceMeetTwo = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);

            try
            {
                // Act
                List<RaceMeet> returnedRaceMeets = dataService.RaceMeet.GetAll();
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedRaceMeets[0], Is.EqualTo(testRaceMeetOne));
                    Assert.That(returnedRaceMeets[1], Is.EqualTo(testRaceMeetTwo));
                    Assert.That(returnedRaceMeets, Has.Count.EqualTo(2));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoRaceMeetsExist_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<RaceMeet> returnedRaceMeets = dataService.RaceMeet.GetAll();

                // Assert
                Assert.That(returnedRaceMeets, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void GetAllAtTrackCalled_IfAnyRaceMeetsAtTrackExist_ReturnAllRaceMeetObjectsAtTrackFromDatabase()
        {
            // Arrange
            const string TEST_TRACK_NAME_ONE = "TestTrackOne";
            const string TEST_TRACK_NAME_TWO = "TestTrackTwo";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME_ONE, TEST_COUNTRY_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME_TWO, TEST_COUNTRY_CODE);
            dataService.Series.CreateNew(TEST_SERIES_NAME);
            
            RaceMeet testRaceMeetOne = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME_ONE, TEST_SERIES_NAME);
            dataService.RaceMeet.CreateNew(TEST_TRACK_NAME_TWO, TEST_SERIES_NAME);

            try
            {
                // Act
                List<RaceMeet> returnedRaceMeets = dataService.RaceMeet.GetAllAtTrack(TEST_TRACK_NAME_ONE);
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedRaceMeets[0], Is.EqualTo(testRaceMeetOne));
                    Assert.That(returnedRaceMeets, Has.Count.EqualTo(1));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllAtTrackCalled_IfNoRaceMeetsAtTrackExist_ReturnEmptyList()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<RaceMeet> returnedRaceMeets = dataService.RaceMeet.GetAllAtTrack(TEST_TRACK_NAME);

                // Assert
                Assert.That(returnedRaceMeets, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
          [Test]
        public void CreateNewCalled_IfTrackWithNameAndSeriesWithNameExist_SaveToDatabaseAndReturnRaceMeetObject()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);
            dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                RaceMeet raceMeet = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);

                // Assert
                bool doesTestRaceMeetExist = dataService.Track.CheckExists(TEST_TRACK_NAME);
                
                Assert.Multiple(() =>
                {
                    Assert.That(doesTestRaceMeetExist, Is.True);
                    Assert.That(raceMeet.TrackName, Is.EqualTo(TEST_TRACK_NAME));
                    Assert.That(raceMeet.SeriesName, Is.EqualTo(TEST_SERIES_NAME));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoTrackWithNameExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();
            
            dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                void CreateNewDelegate() => dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);

                // Assert
                Assert.Throws<KeyNotFoundException>(CreateNewDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoSeriesWithNameExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);

            try
            {
                // Act
                void CreateNewDelegate() => dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);

                // Assert
                Assert.Throws<KeyNotFoundException>(CreateNewDelegate);
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
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";
            const string TEST_RACE_MEET_DESCRIPTION = "TestRaceMeetDescription";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);
            dataService.Series.CreateNew(TEST_SERIES_NAME);
            RaceMeet raceMeet = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);

            raceMeet.Description = TEST_RACE_MEET_DESCRIPTION;

            try
            {
                // Act
                dataService.RaceMeet.Update(raceMeet);

                // Assert
                RaceMeet returnedRaceMeet = dataService.RaceMeet.Get(raceMeet.RaceMeetID);

                Assert.That(raceMeet.Description, Is.EqualTo(returnedRaceMeet.Description));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenNoRaceMeetWithIDExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            var raceMeet = new RaceMeet
            {
                RaceMeetID = Guid.Empty,
                TrackName = TEST_TRACK_NAME,
                SeriesName = TEST_SERIES_NAME
            };

            try
            {
                // Act
                void UpdateDelegate() => dataService.RaceMeet.Update(raceMeet);

                // Assert
                Assert.Throws<KeyNotFoundException>(UpdateDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }
        
        [Test]
        public void DeleteCalled_WhenRaceMeetWithIDFound_RemoveRaceMeetFromDatabase()
        {
            // Arrange
            const string TEST_TRACK_NAME = "TestTrack";
            const string TEST_COUNTRY_CODE = "CountryCode";
            const string TEST_CONTINENT_CODE = "ContinentCode";
            const string TEST_SERIES_NAME = "SeriesName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);
            dataService.Track.CreateNew(TEST_TRACK_NAME, TEST_COUNTRY_CODE);
            dataService.Series.CreateNew(TEST_SERIES_NAME);
            RaceMeet raceMeet = dataService.RaceMeet.CreateNew(TEST_TRACK_NAME, TEST_SERIES_NAME);

            bool doesExistBefore = dataService.RaceMeet.CheckExists(raceMeet.RaceMeetID);

            try
            {
                // Act
                dataService.RaceMeet.Delete(raceMeet.RaceMeetID);

                // Assert
                bool doesExistAfter = dataService.RaceMeet.CheckExists(raceMeet.RaceMeetID);
                
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
        public void DeleteCalled_WhenNoRaceMeetWithIDFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void DeleteDelegate() => dataService.RaceMeet.Delete(Guid.Empty);

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

            string testDdFolderPath = Path.Join(appDataFolderPath, folderStructure);

            return testDdFolderPath;
        }
    }
}
