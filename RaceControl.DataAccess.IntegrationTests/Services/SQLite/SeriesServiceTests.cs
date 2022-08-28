using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.IntegrationTests.Services.SQLite
{
    [TestFixture]
    public class SeriesServiceTests
    {
        [Test]
        public void CheckExistsCalled_IfSeriesWithNameFound_ReturnTrue()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                bool doesExist = dataService.Series.CheckExists(TEST_SERIES_NAME);

                // Assert
                Assert.That(doesExist, Is.True);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoSeriesWithNameFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                bool doesExist = dataService.Series.CheckExists(String.Empty);

                // Assert
                Assert.That(doesExist, Is.False);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfSeriesWithNameFound_ReturnSeriesObjectFromDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            Series testSeries = dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                Series returnedSeries = dataService.Series.Get(TEST_SERIES_NAME);

                // Assert
                Assert.That(testSeries, Is.EqualTo(returnedSeries));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoSeriesWithNameFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void GetSeriesDelegate() => dataService.Series.Get(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(GetSeriesDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfAnySeriesExists_ReturnAllSeriesObjectsFromDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME_ONE = "TestSeriesOne";
            const string TEST_SERIES_NAME_TWO = "TestSeriesTWO";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            Series testSeriesOne = dataService.Series.CreateNew(TEST_SERIES_NAME_ONE);
            Series testSeriesTwo = dataService.Series.CreateNew(TEST_SERIES_NAME_TWO);

            try
            {
                // Act
                List<Series> returnedSeries = dataService.Series.GetAll();
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedSeries[0], Is.EqualTo(testSeriesOne));
                    Assert.That(returnedSeries[1], Is.EqualTo(testSeriesTwo));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoSeriesExists_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<Series> returnedSeries = dataService.Series.GetAll();

                // Assert
                Assert.That(returnedSeries, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoSeriesWithNameExists_SaveToDatabaseAndReturnSeriesObject()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = CreateDataService();           

            try
            {
                // Act
                Series series = dataService.Series.CreateNew(TEST_SERIES_NAME);

                // Assert
                bool doesTestSeriesExist = dataService.Series.CheckExists(TEST_SERIES_NAME);
                
                Assert.Multiple(() =>
                {
                    Assert.That(doesTestSeriesExist, Is.True);
                    Assert.That(series.SeriesName, Is.EqualTo(TEST_SERIES_NAME));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfSeriesWithNameAlreadyExists_ThrowArgumentException()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";
            
            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                void CreateNewDelegate() => dataService.Series.CreateNew(TEST_SERIES_NAME);

                // Assert
                Assert.Throws<ArgumentException>(CreateNewDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenSeriesWithNameAlreadyExists_UpdateSeriesInDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";
            const string TEST_DESCRIPTION = "TestDescription";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            Series series = dataService.Series.CreateNew(TEST_SERIES_NAME);
            series.Description = TEST_DESCRIPTION;

            try
            {
                // Act
                dataService.Series.Update(series);

                // Assert
                Series returnedSeries = dataService.Series.Get(TEST_SERIES_NAME);

                Assert.That(series.Description, Is.EqualTo(returnedSeries.Description));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenNoSeriesWithNameExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";
            const string TEST_DESCRIPTION = "TestDescription";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            Series series = new Series
            {
                SeriesName = TEST_SERIES_NAME,
                Description = TEST_DESCRIPTION
            };

            try
            {
                // Act
                void UpdateDelegate() => dataService.Series.Update(series);

                // Assert
                Assert.Throws<KeyNotFoundException>(UpdateDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenSeriesWithNameFound_RemoveSeriesFromDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Series.CreateNew(TEST_SERIES_NAME);
            
            bool doesExistBefore = dataService.Series.CheckExists(TEST_SERIES_NAME);

            try
            {
                // Act
                dataService.Series.Delete(TEST_SERIES_NAME);

                // Assert
                bool doesExistAfter = dataService.Series.CheckExists(TEST_SERIES_NAME);
                
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
        public void DeleteCalled_WhenNoSeriesWithNameFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void DeleteSeriesDelegate() => dataService.Series.Delete(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(DeleteSeriesDelegate);
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

            string testDbFolderPath = Path.Join(appDataFolderPath, folderStructure);

            return testDbFolderPath;
        }
    }
}
