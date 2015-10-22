using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace AzureApplicationDemo.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send(IEnumerable<HttpPostedFileBase> files)
        {
            return new EmptyResult();
        }
    }
}