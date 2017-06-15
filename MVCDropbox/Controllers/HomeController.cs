using Microsoft.AspNet.Identity;
using System.IO;
using System.Web.Mvc;

namespace MVCDropbox.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            DownloadController dw = new DownloadController();
            ViewBag.CountFiles = dw.GetCountFiles(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName());

            RecycleController rc = new RecycleController();
            ViewBag.CountRecycleFiles = rc.GetCountFiles(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName() + "/Recycle/");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}