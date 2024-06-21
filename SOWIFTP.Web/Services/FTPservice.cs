// SOWI Informatik, www.sowi.ch
// Franz Schönbächler, Juni 2024

// Ignore Spelling: Pservice

using FluentFTP;
using SOWIFTP.Web.Models;

namespace SOWIFTP.Web.Services
{
    /// <summary>
    /// Eine Klasse, die FTP-Dienste bereitstellt und IDisposable implementiert.
    /// </summary>
    public class FTPservice : IDisposable
    {
        /* 
        FluentFTP is a.NET library that provides a simple, fluent and intuitive FTP API.
        https://github.com/robinrodricks/FluentFTP
        
        Example: Async Version
        This code demonstrates how to connect to an FTP server and 
        perform FTP operations using async/await syntax in .NET 4.5 and later.
        https://github.com/robinrodricks/FluentFTP/wiki/Quick-Start-Example#async-version
        */

        /// <summary>
        /// Private Feld zum Speichern der Instanz von AsyncFtpClient.
        /// Dieser Client wird verwendet, um FTP-Operationen asynchron auszuführen.
        /// </summary>
        private readonly AsyncFtpClient _ftpClient;

        /// <summary>
        /// Private Feld zum Speichern des FTP-Verbindungsmodells.
        /// Dieses Modell enthält die erforderlichen Informationen, um eine Verbindung zum FTP-Server herzustellen.
        /// </summary>
        private readonly FTPconnectionModel ftpConnection;

        /// <summary>
        /// Konstruktor für die FTPservice-Klasse.
        /// </summary>
        /// <param name="ftpConnection">Verbindungsdaten für den FTP-Server.</param>
        public FTPservice(FTPconnectionModel ftpConnection)
        {
            this._ftpClient = new AsyncFtpClient(host: ftpConnection.Server,
                                                 user: ftpConnection.Username,
                                                 pass: ftpConnection.Password,
                                                 port: 21);
            this.ftpConnection = ftpConnection;
            this._ftpClient.AutoConnect().Wait(); // Verbindung automatisch herstellen
            this._ftpClient.SetWorkingDirectory(ftpConnection.Path); // Arbeitsverzeichnis setzen
        }

        /// <summary>
        /// Asynchrone Methode zum Abrufen einer Liste von Dateinamen vom FTP-Server.
        /// </summary>
        public async Task<List<string>> GetFilesAsync()
        {
            var ftpListItems = await this._ftpClient.GetListing(this.ftpConnection.Path);
            List<string> files = new();
            foreach (FtpListItem ftpListItem in ftpListItems)
            {
                // if this is a file
                if (ftpListItem.Type == FtpObjectType.File)
                {
                    files.Add(ftpListItem.Name);
                }
            }
            return files;
        }

        /// <summary>
        /// Asynchrone Methode zum Hochladen einer Datei auf den FTP-Server.
        /// </summary>
        public async Task<bool> UploadFileAsync(byte[] fileBytes, string fileName)
        {
            using (var stream = new MemoryStream(fileBytes))
            {
                string fileFullName = Path.Combine(this.ftpConnection.Path, fileName);
                await this._ftpClient.UploadStream(stream, fileFullName);
            }
            return true;
        }

        /// <summary>
        /// Asynchrone Methode zum Löschen einer Datei vom FTP-Server.
        /// </summary>
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            string fileFullName = Path.Combine(this.ftpConnection.Path, fileName);
            await this._ftpClient.DeleteFile(fileFullName);
            return true;
        }

        /// <summary>
        /// Asynchrone Methode zum Abrufen einer Liste von Verzeichnissen vom FTP-Server.
        /// </summary>
        public async Task<List<string>> GetDirectoriesAsync()
        {
            var ftpListItems = await this._ftpClient.GetListing(this.ftpConnection.Path);
            List<string> directories = new();
            foreach (FtpListItem ftpListItem in ftpListItems)
            {
                // if this is a directory
                if (ftpListItem.Type == FtpObjectType.Directory)
                {
                    directories.Add(ftpListItem.Name);
                }
            }
            return directories;
        }

        /// <summary>
        /// Asynchrone Methode zum Abrufen einer Liste von Dateien und Verzeichnissen vom FTP-Server.
        /// </summary>
        public async Task<List<string>> GetFilesAndDirectoriesAsync()
        {
            var ftpListItems = await this._ftpClient.GetListing(this.ftpConnection.Path);
            List<string> content = new();
            foreach (FtpListItem ftpListItem in ftpListItems)
            {
                // if this is a directory
                if (ftpListItem.Type == FtpObjectType.Directory)
                {
                    content.Add(ftpListItem.Name);
                }
                // if this is a file
                if (ftpListItem.Type == FtpObjectType.File)
                {
                    content.Add(ftpListItem.Name);
                }
            }
            return content;
        }

        /// <summary>
        /// Methode zur Freigabe der Ressourcen, die vom FTP-Client verwendet werden.
        /// </summary>
        public void Dispose()
        {
            this._ftpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
