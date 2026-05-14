// SOWI Informatik, www.sowi.ch
// Franz Schönbächler, Juni 2024

// Ignore Spelling: Pservice

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOWIFTP.Web.Models;
using SOWIFTP.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SOWIFTP.Web.Tests
{
    /// <summary>
    /// Test class for FTPservice.
    /// </summary>
    [TestClass]
    public class FTPserviceTests
    {
        private FTPservice? _ftpService;
        private FTPconnectionModel? _ftpConnection;

        // Test variables - names identical to GitHub Secrets and .runsettings
        private static readonly string Server = Environment.GetEnvironmentVariable("TEST_FTP_SERVER") ?? "example.com";
        private static readonly string Username = Environment.GetEnvironmentVariable("TEST_FTP_USERNAME") ?? "testuser";
        private static readonly string Password = Environment.GetEnvironmentVariable("TEST_FTP_PASSWORD") ?? "testpassword";
        private static readonly string Path = Environment.GetEnvironmentVariable("TEST_FTP_PATH") ?? "/";
        private static readonly string TestFileName =
    Environment.GetEnvironmentVariable("TEST_FTP_TEST_FILENAME") ?? "testfile.txt";

        private static readonly string TestFilePath =
    System.IO.Path.Combine(
        Environment.GetEnvironmentVariable("TEST_FTP_TEST_FILEPATH")
            ?? System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(typeof(FTPserviceTests).Assembly.Location)!,
                "TestData"),
        TestFileName);

        /// <summary>
        /// Initializes the FTP connection and service before each test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _ftpConnection = new FTPconnectionModel
            {
                Server = Server,
                Username = Username,
                Password = Password,
                Path = Path
            };

            _ftpService = new FTPservice(_ftpConnection);

            // Testdatei im temp-Verzeichnis erstellen
            File.WriteAllText(TestFilePath, "This is a test file for FTP upload.");
        }

        /// <summary>
        /// Cleans up the FTP service after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _ftpService?.Dispose();

            // Testdatei wieder löschen
            if (File.Exists(TestFilePath))
                File.Delete(TestFilePath);
        }

        /// <summary>
        /// Tests if GetFilesAsync returns a list of files.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task T1_GetFilesAsync_ShouldReturnFilesList()
        {
            // Arrange
            // No specific arrangement needed for this test.

            // Act
            List<string>? files = null;
            if (_ftpService is not null)
            {
                files = await _ftpService.GetFilesAsync();
            }

            // Assert
            Assert.IsNotNull(files);
            Assert.IsInstanceOfType(files, typeof(List<string>));
        }

        /// <summary>
        /// Tests if UploadFileAsync uploads a file successfully.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task T4_UploadFileAsync_ShouldUploadFile()
        {
            // Arrange
            byte[] fileBytes = File.ReadAllBytes(TestFilePath);
            string fileName = TestFileName;

            // Act
            bool isUploaded = false;
            if (_ftpService is not null)
            {
                isUploaded = await _ftpService.UploadFileAsync(fileBytes, fileName);
            }

            // Assert
            Assert.IsTrue(isUploaded);
        }

        /// <summary>
        /// Tests if DeleteFileAsync deletes a file successfully.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task T5_DeleteFileAsync_ShouldDeleteFile()
        {
            // Arrange
            string fileName = TestFileName;

            // Act
            bool isDeleted = false;
            if (_ftpService is not null)
            {
                isDeleted = await _ftpService.DeleteFileAsync(fileName);
            }

            // Assert
            Assert.IsTrue(isDeleted);
        }

        /// <summary>
        /// Tests if GetDirectoriesAsync returns a list of directories.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task T2_GetDirectoriesAsync_ShouldReturnDirectoriesList()
        {
            // Arrange
            // No specific arrangement needed for this test.

            // Act
            List<string>? directories = null;
            if (_ftpService is not null)
            {
                directories = await _ftpService.GetDirectoriesAsync();
            }

            // Assert
            Assert.IsNotNull(directories);
            Assert.IsInstanceOfType(directories, typeof(List<string>));
        }

        /// <summary>
        /// Tests if GetFilesAndDirectoriesAsync returns a list of files and directories.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [TestMethod]
        public async Task T3_GetFilesAndDirectoriesAsync_ShouldReturnFilesAndDirectoriesList()
        {
            // Arrange
            // No specific arrangement needed for this test.

            // Act
            List<string>? content = null;
            if (_ftpService is not null)
            {
                content = await _ftpService.GetFilesAndDirectoriesAsync();
            }

            // Assert
            Assert.IsNotNull(content);
            Assert.IsInstanceOfType(content, typeof(List<string>));
        }
    }
}
