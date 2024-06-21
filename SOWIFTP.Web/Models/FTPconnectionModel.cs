// SOWI Informatik, www.sowi.ch
// Franz Schönbächler, Juni 2024

// Ignore Spelling: Pconnection

namespace SOWIFTP.Web.Models
{
    /// <summary>
    /// Model class representing FTP connection parameters.
    /// </summary>
    public class FTPconnectionModel
    {
        /// <summary>
        /// Gets or sets the FTP server address.
        /// </summary>
        public string? Server { get; set; }

        /// <summary>
        /// Gets or sets the username for FTP authentication.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password for FTP authentication.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the path on the FTP server to access.
        /// Default value is "/" (root directory).
        /// </summary>
        public string Path { get; set; } = "/";
    }
}
