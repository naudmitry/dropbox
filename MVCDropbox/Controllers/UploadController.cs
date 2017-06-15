using Microsoft.AspNet.Identity;
using System.IO;
using System.Web.Mvc;

namespace MVCDropbox.Controllers
{
    public class UploadController : Controller
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

        [Authorize]
        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fullPath = Path.Combine(path + "/", fileName);
                    
                    file.SaveAs(fullPath);
                }
            }

            return RedirectToAction("Index");
        }
        
    }
}