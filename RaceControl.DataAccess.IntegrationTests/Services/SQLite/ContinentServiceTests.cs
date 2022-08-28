using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.IntegrationTests.Services.SQLite
{
    [TestFixture]
    public class ContinentServiceTests
    {
        [Test]
        public void CheckExistsCalled_IfContinentWithCodeFound_ReturnTrue()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                bool doesExist = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);

                // Assert
                Assert.That(doesExist, Is.True);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoContinentWithCodeFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                bool doesExist = dataService.Continent.CheckExists(String.Empty);

                // Assert
                Assert.That(doesExist, Is.False);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfContinentWithCodeFound_ReturnContinentObjectFromDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            Continent testContinent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                Continent returnedContinent = dataService.Continent.Get(TEST_CONTINENT_CODE);

                // Assert
                Assert.That(testContinent, Is.EqualTo(returnedContinent));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoContinentWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void GetSeriesDelegate() => dataService.Continent.Get(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(GetSeriesDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfAnyContinentsExist_ReturnAllContinentObjectsFromDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";
            const string TEST_CONTINENT_CODE_TWO = "TestContinentCodeTwo";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            Continent testContinentOne = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            Continent testContinentTwo = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);

            try
            {
                // Act
                List<Continent> returnedContinents = dataService.Continent.GetAll();
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedContinents[0], Is.EqualTo(testContinentOne));
                    Assert.That(returnedContinents[1], Is.EqualTo(testContinentTwo));
                    Assert.That(returnedContinents, Has.Count.EqualTo(2));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoContinentsExist_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<Continent> returnedContinents = dataService.Continent.GetAll();

                // Assert
                Assert.That(returnedContinents, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoContinentWithCodeExists_SaveToDatabaseAndReturnContinentObject()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

                // Assert
                bool doesTestContinentExist = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);
                
                Assert.Multiple(() =>
                {
                    Assert.That(doesTestContinentExist, Is.True);
                    Assert.That(continent.ContinentCode, Is.EqualTo(TEST_CONTINENT_CODE));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfContinentWithCodeAlreadyExists_ThrowArgumentException()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                void CreateNewDelegate() => dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

                // Assert
                Assert.Throws<ArgumentException>(CreateNewDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenContinentWithCodeAlreadyExists_UpdateContinentInDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";
            const string TEST_NAME = "TestContinent";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            continent.Name = TEST_NAME;

            try
            {
                // Act
                dataService.Continent.Update(continent);

                // Assert
                Continent returnedContinent = dataService.Continent.Get(TEST_CONTINENT_CODE);

                Assert.That(continent.Name, Is.EqualTo(returnedContinent.Name));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenNoContinentWithCodeExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";
            const string TEST_NAME = "TestContinent";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            var continent = new Continent
            {
                ContinentCode = TEST_CONTINENT_CODE,
                Name = TEST_NAME
            };

            try
            {
                // Act
                void UpdateDelegate() => dataService.Continent.Update(continent);

                // Assert
                Assert.Throws<KeyNotFoundException>(UpdateDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenContinentWithCodeFound_RemoveContinentFromDatabase()
        {
            // Arrange
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            bool doesExistBefore = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);

            try
            {
                // Act
                dataService.Continent.Delete(TEST_CONTINENT_CODE);

                // Assert
                bool doesExistAfter = dataService.Continent.CheckExists(TEST_CONTINENT_CODE);
                
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
        public void DeleteCalled_WhenNoContinentWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void DeleteSeriesDelegate() => dataService.Continent.Delete(String.Empty);

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

            string testDdFolderPath = Path.Join(appDataFolderPath, folderStructure);

            return testDdFolderPath;
        }
    }
}
