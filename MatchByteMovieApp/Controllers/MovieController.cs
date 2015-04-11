using System.Security.Claims;
using System.Web.Mvc;

namespace MatchByteMovieApp.Controllers
{
    [RequireHttps]
    [Authorize]
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Index()
        {
            return View();
        }
    }
}