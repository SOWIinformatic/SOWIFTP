// SOWI Informatik, www.sowi.ch
// Franz Schönbächler, Juni 2024

// Ignore Spelling: Pservice

using Microsoft.AspNetCore.Mvc;
using SOWIFTP.Web.Models;
using SOWIFTP.Web.Services;

namespace SOWIFTP.Web.Controllers
{
    /// <summary>
    /// Controller für FTP-Service Operationen.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FTPserviceController : ControllerBase
    {
        /// <summary>
        /// Methode zum Abrufen von Dateien vom FTP-Server.
        /// </summary>
        /// <param name="ftpConnection">FTP-Verbindungsinformationen.</param>
        /// <returns>Liste der Dateinamen, die vom FTP-Server abgerufen wurden.</returns>
        [HttpPost("GetFiles")]
        public async Task<ActionResult<List<string>>> GetFiles([FromBody] FTPconnectionModel ftpConnection)
        {
            using (var ftpService = new FTPservice(ftpConnection))
            {
                var files = await ftpService.GetFilesAsync();
                return Ok(files);
            }
        }

        /// <summary>
        /// Methode zum Hochladen einer Datei auf den FTP-Server.
        /// </summary>
        /// <param name="file">Die zu ladende Datei.</param>
        /// <param name="ftpConnection">FTP-Verbindungsinformationen.</param>
        /// <returns>True, wenn die Datei erfolgreich hochgeladen wurde, ansonsten false.</returns>
        [HttpPost("UploadFile")]
        public async Task<ActionResult<bool>> UploadFile(IFormFile file, [FromForm] FTPconnectionModel ftpConnection)
        {
            using (var ftpService = new FTPservice(ftpConnection))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    var result = await ftpService.UploadFileAsync(fileBytes, file.FileName);
                    return Ok(result);
                }
            }
        }

        /// <summary>
        /// Methode zum Löschen einer Datei auf dem FTP-Server.
        /// </summary>
        /// <param name="fileName">Name der zu löschenden Datei.</param>
        /// <param name="ftpConnection">FTP-Verbindungsinformationen.</param>
        /// <returns>True, wenn die Datei erfolgreich gelöscht wurde, ansonsten false.</returns>
        [HttpDelete("DeleteFile")]
        public async Task<ActionResult<bool>> DeleteFile([FromQuery(Name = "fileName")] string fileName, [FromQuery] FTPconnectionModel ftpConnection)
        {
            using (var ftpService = new FTPservice(ftpConnection))
            {
                var result = await ftpService.DeleteFileAsync(fileName);
                return Ok(result);
            }
        }

        /// <summary>
        /// Methode zum Abrufen von Verzeichnissen vom FTP-Server.
        /// </summary>
        /// <param name="ftpConnection">FTP-Verbindungsinformationen.</param>
        /// <returns>Liste der Verzeichnisnamen, die vom FTP-Server abgerufen wurden.</returns>
        [HttpPost("GetDirectories")]
        public async Task<ActionResult<List<string>>> GetDirectories([FromBody] FTPconnectionModel ftpConnection)
        {
            using (var ftpService = new FTPservice(ftpConnection))
            {
                var directories = await ftpService.GetDirectoriesAsync();
                return Ok(directories);
            }
        }

        /// <summary>
        /// Methode zum Abrufen von Dateien und Verzeichnissen vom FTP-Server.
        /// </summary>
        /// <param name="ftpConnection">FTP-Verbindungsinformationen.</param>
        /// <returns>Liste der Datei- und Verzeichnisnamen, die vom FTP-Server abgerufen wurden.</returns>
        [HttpPost("GetFilesAndDirectories")]
        public async Task<ActionResult<List<string>>> GetFilesAndDirectories([FromBody] FTPconnectionModel ftpConnection)
        {
            using (var ftpService = new FTPservice(ftpConnection))
            {
                var filesAndDirs = await ftpService.GetFilesAndDirectoriesAsync();
                return Ok(filesAndDirs);
            }
        }
    }
}
