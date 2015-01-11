using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mikkelhm.Web.Controllers
{
    public class LegacyFilesController : Controller
    {
        public ActionResult ShowImage(string picture)
        {
            var legacyFilesFolder = Path.Combine(Server.MapPath("/"), "App_Data\\Legacy_files");
            var filePath = Path.Combine(legacyFilesFolder, picture.Remove(0, 1));
            if (!System.IO.File.Exists(filePath))
                return new HttpNotFoundResult();

            return base.File(filePath, MimeMapping.GetMimeMapping(filePath));
        }

        public ActionResult DownloadFile(string year, string month, string filename)
        {
            var legacyFilesFolder = Path.Combine(Server.MapPath("/"), "App_Data\\Legacy_files");
            var filePath = Path.Combine(legacyFilesFolder, String.Format("{0}\\{1}\\{2}", year, month, filename));
            if (!System.IO.File.Exists(filePath))
                return new HttpNotFoundResult();

            var result = new FileStreamResult(new FileStream(filePath, FileMode.Open),
                MimeMapping.GetMimeMapping(filePath));
            result.FileDownloadName = filename;
            return result;
        }
    }
}
