using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCDataAccessIntegrationTests.Services
{
    [TestFixture]
    public class SQLiteDataService_SeriesService
    {
        [Test]
        public void CheckExistsCalled_IfSeriesWithNameFound_ReturnTrue()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = createDataService();

            Series series = dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                bool doesExist = dataService.Series.CheckExists(TEST_SERIES_NAME);

                // Assert
                Assert.IsTrue(doesExist);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoSeriesWithNameFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                bool doesExist = dataService.Series.CheckExists(String.Empty);

                // Assert
                Assert.IsFalse(doesExist);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfSeriesWithNameFound_ReturnSeriesObjectFromDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = createDataService();

            Series testSeries = dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                Series returnedSeries = dataService.Series.Get(TEST_SERIES_NAME);

                // Assert
                Assert.That(testSeries == returnedSeries);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoSeriesWithNameFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate getSeriesDelegate = () => dataService.Series.Get(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(getSeriesDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void AllCalled_IfAnySeriesExists_ReturnAllSeriesObjectsFromDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME_ONE = "TestSeriesOne";
            const string TEST_SERIES_NAME_TWO = "TestSeriesTWO";

            (IDataService dataService, string testFolderPath) = createDataService();

            Series testSeriesOne = dataService.Series.CreateNew(TEST_SERIES_NAME_ONE);
            Series testSeriesTwo = dataService.Series.CreateNew(TEST_SERIES_NAME_TWO);

            try
            {
                // Act
                List<Series> returnedSeries = dataService.Series.GetAll();

                // Assert
                Assert.That(returnedSeries[0] == testSeriesOne);
                Assert.That(returnedSeries[1] == testSeriesTwo);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoSeriesExists_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                List<Series> returnedSeries = dataService.Series.GetAll();

                // Assert
                Assert.That(returnedSeries.Count == 0);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoSeriesWithNameExists_SaveToDatabaseAndReturnSeriesObject()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = createDataService();           

            try
            {
                // Act
                Series series = dataService.Series.CreateNew(TEST_SERIES_NAME);

                // Assert
                bool doesTestSeriesExist = dataService.Series.CheckExists(TEST_SERIES_NAME);

                Assert.IsTrue(doesTestSeriesExist);
                Assert.That(series.SeriesName == TEST_SERIES_NAME);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfSeriesWithNameAlreadyExists_ThrowArgumentException()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";
            
            (IDataService dataService, string testFolderPath) = createDataService();

            Series series = dataService.Series.CreateNew(TEST_SERIES_NAME);

            try
            {
                // Act
                TestDelegate createNewDelegate = () => dataService.Series.CreateNew(TEST_SERIES_NAME);

                // Assert
                bool doesTestSeriesExist = dataService.Series.CheckExists(TEST_SERIES_NAME);

                Assert.Throws<ArgumentException>(createNewDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenSeriesWithNameAlreadyExists_UpdateSeriesInDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";
            const string TEST_DESCRIPTION = "TestDescription";

            (IDataService dataService, string testFolderPath) = createDataService();

            Series series = dataService.Series.CreateNew(TEST_SERIES_NAME);
            series.Description = TEST_DESCRIPTION;

            try
            {
                // Act
                dataService.Series.Update(series);

                // Assert
                Series returnedSeries = dataService.Series.Get(TEST_SERIES_NAME);

                Assert.That(series.Description == returnedSeries.Description);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenNoSeriesWithNameExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";
            const string TEST_DESCRIPTION = "TestDescription";

            (IDataService dataService, string testFolderPath) = createDataService();

            Series series = new Series();
            series.SeriesName = TEST_SERIES_NAME;
            series.Description = TEST_DESCRIPTION;

            try
            {
                // Act
                TestDelegate updateDelegate = () => dataService.Series.Update(series);

                // Assert
                Assert.Throws<KeyNotFoundException>(updateDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenSeriesWithNameFound_RemoveSeriesFromDatabase()
        {
            // Arrange
            const string TEST_SERIES_NAME = "TestSeries";

            (IDataService dataService, string testFolderPath) = createDataService();

            Series testSeries = dataService.Series.CreateNew(TEST_SERIES_NAME);
            
            bool doesExistBefore = dataService.Series.CheckExists(TEST_SERIES_NAME);

            try
            {
                // Act
                dataService.Series.Delete(TEST_SERIES_NAME);

                // Assert
                bool doesExistAfter = dataService.Series.CheckExists(TEST_SERIES_NAME);

                Assert.IsTrue(doesExistBefore);
                Assert.IsFalse(doesExistAfter);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenNoSeriesWithNameFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate deleteSeriesDelegate = () => dataService.Series.Delete(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(deleteSeriesDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        private (IDataService, string) createDataService()
        {
            const string TEST_DB_NAME = "testdatabase";

            string testFolderStructure = String.Format("RCDataAccessTests{0}", Guid.NewGuid());
            string testFolderPath = getTestDBFolderPath(testFolderStructure);

            return (new DataService(TEST_DB_NAME, testFolderStructure), testFolderPath);
        }

        private void cleanUpDataService(IDataService dataService, string testFolderPath)
        {
            if (dataService != null)
            {
                dataService.DeleteSource();
                dataService.Dispose();
                Directory.Delete(testFolderPath, true);
            }
        }

        private string getTestDBFolderPath(string folderStructure)
        {
            Environment.SpecialFolder appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string appDataFolderPath = Environment.GetFolderPath(appDataFolder);

            string testDBFolderPath = Path.Join(appDataFolderPath, folderStructure);

            return testDBFolderPath;
        }
    }
}
