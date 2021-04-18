using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace GTGrimServer.Results
{
    [DefaultStatusCode(DefaultStatusCode)]
    public class BanResult : StatusCodeResult
    {
        public const int DefaultStatusCode = 499;
        public BanResult() : base(DefaultStatusCode) { }
    }
}
