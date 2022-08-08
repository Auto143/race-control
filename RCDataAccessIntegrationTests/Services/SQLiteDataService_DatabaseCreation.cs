using RCDataAccess.Services.Interfaces;
using RCDataAccess.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCDataAccess.Services.Implementations.SQLite;

namespace RCDataAccessIntegrationTests.ServiceTests
{
    [TestFixture]
    [NonParallelizable]
    public class SQLiteDataService_DatabaseCreation
    {
        [Test]
        public void OnInstantiation_CreateFileDatabase_IfDoesntExist()
        {
            // Arrange
            const string TEST_DB_NAME = "testdatabase";

            string testFolderStructure = String.Format("RCDataAccessTests{0}", DateTime.Now.ToString().Replace("/", "").Replace(":", ""));
            string testFolderPath = getTestDBFolderPath(testFolderStructure);
            string databaseFilePath = Path.Join(testFolderPath, String.Format("{0}.db", TEST_DB_NAME));

            SQLiteDataService? dataService = null;

            try
            {
                // Act
                dataService = new SQLiteDataService(TEST_DB_NAME, testFolderStructure);

                // Assert
                bool doesDBFileExist = File.Exists(databaseFilePath);
                Assert.IsTrue(doesDBFileExist);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        [Test]
        public void OnInstantiation_DontRecreateFileDatabase_IfAlreadyExists()
        {
            //Arrange
            const string TEST_DB_NAME = "testdatabase";
            const int WAIT_TIME_BETWEEN_CHECKS = 1000;

            string testFolderStructure = String.Format("RCDataAccessTests{0}", DateTime.Now.ToString().Replace("/", "").Replace(":", ""));
            string testFolderPath = getTestDBFolderPath(testFolderStructure);
            string databaseFilePath = Path.Join(testFolderPath, String.Format("{0}.db", TEST_DB_NAME));

            SQLiteDataService? dataService = null;

            dataService = new SQLiteDataService(TEST_DB_NAME, testFolderStructure);
            dataService.Dispose();
            dataService = null;

            try
            {
                // Act
                long beforeLastFileWriteTime = File.GetLastWriteTime(databaseFilePath).Ticks;
                dataService = new SQLiteDataService(TEST_DB_NAME, testFolderStructure);

                Thread.Sleep(WAIT_TIME_BETWEEN_CHECKS);
                long afterLastFileWriteTime = File.GetLastWriteTime(databaseFilePath).Ticks;

                // Assert
                Assert.That(beforeLastFileWriteTime == afterLastFileWriteTime);
            }
            finally
            {
                cleanUpDataService(dataService, testFolderPath);
            }
        }

        private void cleanUpDataService(IDataService? dataService, string testFolderPath)
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
