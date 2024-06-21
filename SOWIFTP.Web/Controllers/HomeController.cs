// SOWI Informatik, www.sowi.ch
// Franz Schönbächler, Juni 2024

using Microsoft.AspNetCore.Mvc;
using SOWIFTP.Web.Models;
using System.Diagnostics;

namespace SOWIFTP.Web.Controllers
{
    /// <summary>
    /// Controller for handling home page and error views.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        public HomeController() { }

        /// <summary>
        /// Action method for handling requests to the default route (usually the homepage).
        /// </summary>
        /// <returns>The view result for the homepage.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action method to handle errors.
        /// </summary>
        /// <returns>The view result for the error page.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Generating ErrorViewModel instance with request ID or trace identifier
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
