using System;
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
            var batchId = Guid.NewGuid();
            foreach (var file in files)
            {
                var message = new UploadFileCommand
                {
                    FileName = file.FileName,
                    Stream = file.InputStream,
                    ContentLength = file.ContentLength,
                    BatchId = batchId
                };
                _mediator.Send(message);
            }
            _mediator.Publish(new BatchUploadComplete { BatchId = batchId });
            return new EmptyResult();
        }
    }
}