using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GTGrimServer.Results
{
    [DefaultStatusCode(DefaultStatusCode)]
    public class ConsoleBanObjectResult : ObjectResult
    {
        public const int DefaultStatusCode = 497;
        public ConsoleBanObjectResult(object value)
            : base(value)
        {
            StatusCode = DefaultStatusCode;
        }
    }
}
