using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace MVCDropbox.Controllers
{
    public class DownloadController : Controller
    {
        [Authorize]
        public ActionResult Index(string searchString, int? page)
        {
            ViewBag.CurrentFilter = searchString;
            ViewBag.CountFiles = GetCountFiles(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName());

            RecycleController rc = new RecycleController();
            ViewBag.CountRecycleFiles = rc.GetCountFiles(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName() +"/Recycle/");

            var directoryInfo = new DirectoryInfo(GetPathToFiles());

            if (directoryInfo.GetFiles().Length < 0)
            {
                return View();
            }

            List<string> items = new List<string>();
            FileInfo[] fileNames = directoryInfo.GetFiles("*.*");

            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            return View(items.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public FileResult Download(string ImageName)
        {
            return File(GetPathToFiles() + ImageName,
                System.Net.Mime.MediaTypeNames.Application.Octet, ImageName);
        }

        [Authorize]
        public ActionResult Delete(string ImageName)
        {
            var path = GetPathToFiles() + ImageName;
            var path2 = GetPathToFiles() + "Recycle/" + ImageName;
            
            System.IO.File.Move(path, path2);

            return RedirectToAction("Index");
        }

        public string GetPathToFiles()
        {
            var path = Path.Combine(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName() + "/");

            return path;
        }

        public int GetCountFiles(string MapPath, string UserName)
        {
            var path = Path.Combine(MapPath, UserName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var directoryInfo = new DirectoryInfo(path);

            return directoryInfo.GetFiles().Length;
        }
    }
}