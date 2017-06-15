using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MVCDropbox.Controllers
{
    public class RecycleController : Controller
    {
        [Authorize]
        public ActionResult Index(string searchString, int? page)
        {
            ViewBag.CurrentFilter = searchString;

            DownloadController dw = new DownloadController();
            ViewBag.CountFiles = dw.GetCountFiles(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName());

            ViewBag.CountRecycleFiles = GetCountFiles(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName() + "/Recycle");

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

            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            return View(items.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Delete(string ImageName)
        {
            System.IO.File.Delete(GetPathToFiles() + ImageName);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Restore(string ImageName)
        {
            var path = GetPathToFiles() + ImageName;
            var path2 = Path.Combine(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName() + "/" + ImageName);

            System.IO.File.Move(path, path2);

            return RedirectToAction("Index");
        }

        public string GetPathToFiles()
        {
            var path = Path.Combine(Server.MapPath("~/UsersFiles/"), User.Identity.GetUserName() + "/Recycle/");

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