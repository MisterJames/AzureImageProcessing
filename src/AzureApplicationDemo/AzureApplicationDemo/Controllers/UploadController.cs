using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AzureApplicationDemo.Features.Upload;
using MediatR;

namespace AzureApplicationDemo.Controllers
{
    public class UploadController : Controller
    {
        private readonly IMediator _mediator;
        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send(IEnumerable<HttpPostedFileBase> files)
        {
            foreach(var file in files)
            {
                var message = new UploadFileCommand { FileName = file.FileName, Stream = file.InputStream, ContentLength = file.ContentLength };
                _mediator.Send(message);
            }
            return new EmptyResult();
        }
    }
}