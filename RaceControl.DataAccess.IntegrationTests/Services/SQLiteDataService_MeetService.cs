﻿using RaceControl.DataAccess.Services.Implementations.SQLite;
using RaceControl.DataAccess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceControl.DataAccess.IntegrationTests.Services
{
    [TestFixture]
    public class SQLiteDataService_MeetService
    {
        [Test]
        public void WhenGetCalled_ThrowKeyNotFoundException_WhenNoMeetWithIDFound()
        {
            // Arrange
            (IDataService dataService, string testFolderPath) = createDataService();

            try
            {
                // Act
                TestDelegate meetDelegate = () => dataService.Meet.Get(new Guid());

                // Assert
                Assert.Throws<KeyNotFoundException>(meetDelegate);
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