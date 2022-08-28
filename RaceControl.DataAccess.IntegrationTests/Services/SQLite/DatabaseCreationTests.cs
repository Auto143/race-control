using System.Globalization;
using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;

namespace RaceControl.DataAccess.IntegrationTests.Services.SQLite
{
    [TestFixture]
    [NonParallelizable]
    public class DatabaseCreationTests
    {
        [Test]
        public void OnInstantiation_CreateFileDatabase_IfDoesntExist()
        {
            // Arrange
            const string TEST_DB_NAME = "TestDatabase";

            string testFolderStructure = 
                $"RCDataAccessTests{DateTime.Now.ToString(CultureInfo.CurrentCulture).Replace("/", "").Replace(":", "")}";
            string testFolderPath = GetTestDbFolderPath(testFolderStructure);
            string databaseFilePath = Path.Join(testFolderPath, $"{TEST_DB_NAME}.db");

            DataService? dataService = null;

            try
            {
                // Act
                dataService = new DataService(TEST_DB_NAME, testFolderStructure);

                // Assert
                bool doesDatabaseFileExist = File.Exists(databaseFilePath);
                Assert.That(doesDatabaseFileExist, Is.True);
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void OnInstantiation_DontRecreateFileDatabase_IfAlreadyExists()
        {
            //Arrange
            const string TEST_DB_NAME = "TestDatabase";
            const int WAIT_TIME_BETWEEN_CHECKS = 1000;

            string testFolderStructure =
                $"RCDataAccessTests{DateTime.Now.ToString(CultureInfo.CurrentCulture).Replace("/", "").Replace(":", "")}";
            string testFolderPath = GetTestDbFolderPath(testFolderStructure);
            string databaseFilePath = Path.Join(testFolderPath, $"{TEST_DB_NAME}.db");

            var dataService = new DataService(TEST_DB_NAME, testFolderStructure);
            dataService.Dispose();
            dataService = null;

            try
            {
                // Act
                long beforeLastFileWriteTime = File.GetLastWriteTime(databaseFilePath).Ticks;
                dataService = new DataService(TEST_DB_NAME, testFolderStructure);

                Thread.Sleep(WAIT_TIME_BETWEEN_CHECKS);
                long afterLastFileWriteTime = File.GetLastWriteTime(databaseFilePath).Ticks;

                // Assert
                Assert.That(beforeLastFileWriteTime, Is.EqualTo(afterLastFileWriteTime));
            }
            finally
            {
                CleanUpDataService(dataService, testFolderPath);
            }
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
