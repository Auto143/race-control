using RaceControl.DataAccess.Models;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.IntegrationTests.Services.SQLite
{
    [TestFixture]
    public class CountryServiceTests
    {
        [Test]
        public void CheckExistsCalled_IfCountryWithCodeFound_ReturnTrue()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            try
            {
                // Act
                bool doesExist = dataService.Country.CheckExists(TEST_COUNTRY_CODE);

                // Assert
                Assert.That(doesExist, Is.True);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoCountryWithCodeFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                bool doesExist = dataService.Country.CheckExists(String.Empty);

                // Assert
                Assert.That(doesExist, Is.False);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfCountryWithCodeFound_ReturnCountryObjectFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            Country testCountry = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            try
            {
                // Act
                Country returnedCountry = dataService.Country.Get(TEST_COUNTRY_CODE);

                // Assert
                Assert.That(testCountry, Is.EqualTo(returnedCountry));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoCountryWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void GetSeriesDelegate() => dataService.Country.Get(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(GetSeriesDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfAnyCountriesExist_ReturnAllCountryObjectsFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE_ONE = "TestCountryCodeOne";
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";

            const string TEST_COUNTRY_CODE_TWO = "TestCountryCodeTwo";
            const string TEST_CONTINENT_CODE_TWO = "TestContinentCodeTwo";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            Country testCountryOne = dataService.Country.CreateNew(TEST_COUNTRY_CODE_ONE, TEST_CONTINENT_CODE_ONE);

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);
            Country testCountryTwo = dataService.Country.CreateNew(TEST_COUNTRY_CODE_TWO, TEST_CONTINENT_CODE_TWO);

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAll();
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedCountries[0], Is.EqualTo(testCountryOne));
                    Assert.That(returnedCountries[1], Is.EqualTo(testCountryTwo));
                    Assert.That(returnedCountries, Has.Count.EqualTo(2));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoCountriesExist_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAll();

                // Assert
                Assert.That(returnedCountries, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllInContinentCalled_IfAnyCountriesInContinentExist_ReturnAllCountryObjectsInContinentFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE_ONE = "TestCountryCodeOne";
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";

            const string TEST_COUNTRY_CODE_TWO = "TestCountryCodeTwo";
            const string TEST_CONTINENT_CODE_TWO = "TestContinentCodeTwo";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            Country testCountryOne = dataService.Country.CreateNew(TEST_COUNTRY_CODE_ONE, TEST_CONTINENT_CODE_ONE);

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE_TWO, TEST_CONTINENT_CODE_TWO);

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAllInContinent(TEST_CONTINENT_CODE_ONE);
                
                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(returnedCountries[0], Is.EqualTo(testCountryOne));
                    Assert.That(returnedCountries, Has.Count.EqualTo(1));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllInContinentCalled_IfNoCountriesInContinentExist_ReturnEmptyList()
        {
            // Arrange
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAllInContinent(TEST_CONTINENT_CODE_ONE);

                // Assert
                Assert.That(returnedCountries, Is.Empty);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoCountryWithCodeExistsAndContinentWithCodeExists_SaveToDatabaseAndReturnCountryObject()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                Country country = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

                // Assert
                bool doesTestCountryExist = dataService.Country.CheckExists(TEST_COUNTRY_CODE);
                
                Assert.Multiple(() =>
                {
                    Assert.That(doesTestCountryExist, Is.True);
                    Assert.That(country.CountryCode, Is.EqualTo(TEST_COUNTRY_CODE));
                });
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoCountryWithCodeExistsAndContinentWithCodeDoesntExist_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void CreateNewDelegate() => dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

                // Assert
                Assert.Throws<KeyNotFoundException>(CreateNewDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfCountryWithCodeAlreadyExists_ThrowArgumentException()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            try
            {
                // Act
                void CreateNewDelegate() => dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

                // Assert
                Assert.Throws<ArgumentException>(CreateNewDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenCountryWithCodeAlreadyExists_UpdateContinentInDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_NAME = "TestCountryName";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            Country country = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            country.Name = TEST_NAME;

            try
            {
                // Act
                dataService.Country.Update(country);

                // Assert
                Country returnedCountry = dataService.Country.Get(TEST_COUNTRY_CODE);

                Assert.That(country.Name, Is.EqualTo(returnedCountry.Name));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenNoCountryWithCodeExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_NAME = "TestCountryName";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            var country = new Country
            {
                CountryCode = TEST_COUNTRY_CODE,
                Name = TEST_NAME
            };

            try
            {
                // Act
                void UpdateDelegate() => dataService.Country.Update(country);

                // Assert
                Assert.Throws<KeyNotFoundException>(UpdateDelegate);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenCountryWithCodeFound_RemoveCountryFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = CreateDataService();

            dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

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
        public void DeleteCalled_WhenNoCountryWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = CreateDataService();

            try
            {
                // Act
                void DeleteSeriesDelegate() => dataService.Country.Delete(String.Empty);

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
