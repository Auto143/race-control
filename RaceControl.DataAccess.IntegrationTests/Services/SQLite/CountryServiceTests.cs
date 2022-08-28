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

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            Country country = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            try
            {
                // Act
                bool doesExist = dataService.Country.CheckExists(TEST_COUNTRY_CODE);

                // Assert
                Assert.IsTrue(doesExist);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CheckExistsCalled_IfNoCountryWithCodeFound_ReturnFalse()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                bool doesExist = dataService.Country.CheckExists(String.Empty);

                // Assert
                Assert.IsFalse(doesExist);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfCountryWithCodeFound_ReturnCountryObjectFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            Country testCountry = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            try
            {
                // Act
                Country returnedCountry = dataService.Country.Get(TEST_COUNTRY_CODE);

                // Assert
                Assert.That(testCountry == returnedCountry);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetCalled_IfNoCountryWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate getSeriesDelegate = () => dataService.Country.Get(String.Empty);

                // Assert
                Assert.Throws<KeyNotFoundException>(getSeriesDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
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

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continentOne = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            Country testCountryOne = dataService.Country.CreateNew(TEST_COUNTRY_CODE_ONE, TEST_CONTINENT_CODE_ONE);

            Continent ContinentTwo = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);
            Country testCountryTwo = dataService.Country.CreateNew(TEST_COUNTRY_CODE_TWO, TEST_CONTINENT_CODE_TWO);

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAll();

                // Assert
                Assert.That(returnedCountries[0] == testCountryOne);
                Assert.That(returnedCountries[1] == testCountryTwo);

                Assert.That(returnedCountries.Count == 2);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllCalled_IfNoCountriesExist_ReturnEmptyList()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAll();

                // Assert
                Assert.That(returnedCountries.Count == 0);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
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

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continentOne = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_ONE);
            Country testCountryOne = dataService.Country.CreateNew(TEST_COUNTRY_CODE_ONE, TEST_CONTINENT_CODE_ONE);

            Continent ContinentTwo = dataService.Continent.CreateNew(TEST_CONTINENT_CODE_TWO);
            Country testCountryTwo = dataService.Country.CreateNew(TEST_COUNTRY_CODE_TWO, TEST_CONTINENT_CODE_TWO);

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAllInContinent(TEST_CONTINENT_CODE_ONE);

                // Assert
                Assert.That(returnedCountries[0] == testCountryOne);

                Assert.That(returnedCountries.Count == 1);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void GetAllInContinentCalled_IfNoCountriesInContinentExist_ReturnEmptyList()
        {
            // Arrange
            const string TEST_CONTINENT_CODE_ONE = "TestContinentCodeOne";

            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                List<Country> returnedCountries = dataService.Country.GetAllInContinent(TEST_CONTINENT_CODE_ONE);

                // Assert
                Assert.That(returnedCountries.Count == 0);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoCountryWithCodeExistsAndContinentWithCodeExists_SaveToDatabaseAndReturnCountryObject()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);

            try
            {
                // Act
                Country country = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

                // Assert
                bool doesTestCountryExist = dataService.Country.CheckExists(TEST_COUNTRY_CODE);

                Assert.IsTrue(doesTestCountryExist);
                Assert.That(country.CountryCode == TEST_COUNTRY_CODE);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfNoCountryWithCodeExistsAndContinentWithCodeDoesntExist_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate createNewDelegate = () => dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

                // Assert
                Assert.Throws<KeyNotFoundException>(createNewDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void CreateNewCalled_IfCountryWithCodeAlreadyExists_ThrowArgumentException()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            Country country = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            try
            {
                // Act
                TestDelegate createNewDelegate = () => dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

                // Assert

                Assert.Throws<ArgumentException>(createNewDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenCountryWithCodeAlreadyExists_UpdateContinentInDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_NAME = "TestCountryName";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            Country country = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            country.Name = TEST_NAME;

            try
            {
                // Act
                dataService.Country.Update(country);

                // Assert
                Country returnedCountry = dataService.Country.Get(TEST_COUNTRY_CODE);

                Assert.That(country.Name == returnedCountry.Name);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void UpdateCalled_WhenNoCountryWithCodeExists_ThrowKeyNotFoundException()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_NAME = "TestCountryName";

            (IDataService dataService, string testFolderPath) = createDataService();

            var country = new Country();
            country.CountryCode = TEST_COUNTRY_CODE;
            country.Name = TEST_NAME;

            try
            {
                // Act
                TestDelegate updateDelegate = () => dataService.Country.Update(country);

                // Assert
                Assert.Throws<KeyNotFoundException>(updateDelegate);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenCountryWithCodeFound_RemoveCountryFromDatabase()
        {
            // Arrange
            const string TEST_COUNTRY_CODE = "TestCountryCode";
            const string TEST_CONTINENT_CODE = "TestContinentCode";

            (IDataService dataService, string testFolderPath) = createDataService();

            Continent continent = dataService.Continent.CreateNew(TEST_CONTINENT_CODE);
            Country testCountry = dataService.Country.CreateNew(TEST_COUNTRY_CODE, TEST_CONTINENT_CODE);

            bool doesExistBefore = dataService.Country.CheckExists(TEST_COUNTRY_CODE);

            try
            {
                // Act
                dataService.Country.Delete(TEST_COUNTRY_CODE);

                // Assert
                bool doesExistAfter = dataService.Country.CheckExists(TEST_COUNTRY_CODE);

                Assert.IsTrue(doesExistBefore);
                Assert.IsFalse(doesExistAfter);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void DeleteCalled_WhenNoCountryWithCodeFound_ThrowKeyNotFoundException()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate deleteSeriesDelegate = () => dataService.Country.Delete(String.Empty);

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
