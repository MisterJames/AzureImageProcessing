using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediatR;

namespace AzureApplicationDemo.Features.Upload
{
    public class UploadFileCommand:IRequest
    {
        public string FileName { get; set; }
        public System.IO.Stream Stream { get; set; }
        public int ContentLength { get; set; }

    }
}