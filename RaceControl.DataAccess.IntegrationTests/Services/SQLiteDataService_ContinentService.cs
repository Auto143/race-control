using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.IntegrationTests.Services
{
    [TestFixture]
    public class SQLiteDataService_ContinentService
    {
        [Test]
        public void CheckExistsCalled_IfContinentWithCodeFound_ReturnTrue()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                bool doesExist = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);

                // Assert
                Assert.IsTrue(doesExist);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoContinentWithCodeFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                bool doesExist = dataService.Continent.CheckExists(String.Empty);

                // Assert
                Assert.IsFalse(doesExist);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfContinentWithCodeFound_ReturnContinentObjectFromDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent testContinent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                Continent returnedContinent = dataService.Continent.Get(TEST_CONTINENT_CODE);

                // Assert
                Assert.That(testContinent == returnedContinent);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoContinentWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate getSeriesDelegate = () => dataService.Continent.Get(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(getSeriesDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfAnyContinentsExist_ReturnAllContinentObjectsFromDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";
            const string TEST_CONTINENT_CODE_TWO = "TestContinentCodeTwo";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent testContinentOne = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            Continent testContinentTwo = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);

            try
            {
                // Act
                List<Continent> returnedContinents = dataService.Continent.GetAll();

                // Assert
                Assert.That(returnedContinents[0] == testContinentOne);
                Assert.That(returnedContinents[1] == testContinentTwo);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoContinentsExist_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                List<Continent> returnedContinents = dataService.Continent.GetAll();

                // Assert
                Assert.That(returnedContinents.Count == 0);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoContinentWithCodeExists_SaveToDatabaseAndReturnContinentObject()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

                // Assert
                bool doesTestContinentExist = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);

                Assert.IsTrue(doesTestContinentExist);
                Assert.That(continent.ContinentCode == TEST_CONTINENT_CODE);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfContinentWithCodeAlreadyExists_ThrowArgumentException()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                TestDelegate createNewDelegate = () => dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

                // Assert
                bool doesTestSeriesExist = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);

                Assert.Throws<ArgumentException>(createNewDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenContinentWithCodeAlreadyExists_UpdateContinentInDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";
            const string TEST_NAME = "TestContinent";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            continent.Name = TEST_NAME;

            try
            {
                // Act
                dataService.Continent.Update(continent);

                // Assert
                Continent returnedContinent = dataService.Continent.Get(TEST_CONTINENT_CODE);

                Assert.That(continent.Name == returnedContinent.Name);
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
            const string TEST_CONTINENT_CODE = "TestContinentCode";
            const string TEST_NAME = "TestContinent";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = new Continent();
            continent.ContinentCode = TEST_CONTINENT_CODE;
            continent.Name = TEST_NAME;

            try
            {
                // Act
                TestDelegate updateDelegate = () => dataService.Continent.Update(continent);

                // Assert
                Assert.Throws<KeyNotFoundException>(updateDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenContinentWithCodeFound_RemoveContinentFromDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent testContinent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            bool doesExistBefore = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);

            try
            {
                // Act
                dataService.Continent.Delete(TEST_CONTINENT_CODE);

                // Assert
                bool doesExistAfter = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);

                Assert.IsTrue(doesExistBefore);
                Assert.IsFalse(doesExistAfter);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenNoContinentWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate deleteSeriesDelegate = () => dataService.Continent.Delete(String.Empty);

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
