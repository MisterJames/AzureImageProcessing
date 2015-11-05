using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediatR;

namespace AzureApplicationDemo.Features.Onboarding
{
    public class QueueHostCommand : IRequest
    {
        public QueueHostViewModel Host { get; set; }
    }
}