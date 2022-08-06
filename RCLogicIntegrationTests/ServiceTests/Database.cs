using RCLogic.DataAccess.Services.Interfaces;
using RCLogic.DataAccess.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCLogicIntegrationTests.ServiceTests
{
    [TestFixture]
    public class RCDBContext_DatabaseCreation
    {
        private const string TEST_FOLDER = @"\RCLogicTest\";

        [Test]
        public void OnInitialization_CreateFileDatabase_IfDoesntExist()
        {
            IDataService? databaseService = null;

            try
            {
                // Arrange
                const string TEST_DB_FOLDER = @"Database\";
                const string TEST_DB_NAME = "testdatabase";
                string testFolderStructure = Path.Join(TEST_FOLDER, TEST_DB_FOLDER);

                // Act
                databaseService = new SQLiteDataService(TEST_DB_NAME, testFolderStructure);

                // Assert
                bool doesDBFileExist = File.Exists(Path.Join(GetTestDBFolderPath(testFolderStructure), String.Format("{0}.db", TEST_DB_NAME)));
                Assert.IsTrue(doesDBFileExist);
            }
            finally
            {
                if (databaseService != null)
                {
                    databaseService.DeleteSource();
                    databaseService.Dispose();
                }
            }
        }

        [Test]
        public async Task OnInitialization_DontRecreateFileDatabase_IfAlreadyExists()
        {
            IDataService? databaseService = null;

            try
            {
                //Arrange
                const string TEST_DB_FOLDER = @"Database\";
                const string TEST_DB_NAME = "testdatabase";
                const int WAIT_TIME_AVOID_FALSE_POSITIVE = 1000;

                string testFolderStructure = Path.Join(TEST_FOLDER, TEST_DB_FOLDER);
                string testDBFilePath = Path.Join(GetTestDBFolderPath(testFolderStructure), String.Format("{0}.db", TEST_DB_NAME));

                databaseService = new SQLiteDataService(TEST_DB_NAME, testFolderStructure);
                databaseService.Dispose();
                databaseService = null;

                // Act
                DateTime beforeLastFileWriteTime = File.GetLastWriteTime(testDBFilePath);
                await Task.Delay(WAIT_TIME_AVOID_FALSE_POSITIVE);

                databaseService = new SQLiteDataService(TEST_DB_NAME, testFolderStructure);
                DateTime afterLastFileWriteTime = File.GetLastWriteTime(testDBFilePath);

                // Assert
                Assert.That(beforeLastFileWriteTime == afterLastFileWriteTime);
            }
            finally
            {
                if (databaseService != null)
                {
                    databaseService.DeleteSource();
                    databaseService.Dispose();
                }
            }
        }

        [TearDown]
        public void CleanUpFiles()
        {
            string testDBFolderPath = GetTestDBFolderPath(TEST_FOLDER);
            if (Directory.Exists(testDBFolderPath) == true)
            {
                Directory.Delete(testDBFolderPath, true);
            }
        }

        private string GetTestDBFolderPath(string folderStructure)
        {
            Environment.SpecialFolder appDataFolder = Environment.SpecialFolder.LocalApplicationData;
            string appDataFolderPath = Environment.GetFolderPath(appDataFolder);

            string testDBFolderPath = Path.Join(appDataFolderPath, folderStructure);

            return testDBFolderPath;
        }
    }
}
