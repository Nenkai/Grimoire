using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace GTGrimServer.Utils
{
    public static class StaticFileUtil
    {
        public static async Task SendFile(this ControllerBase cBase, string mainDir, string fileName)
        {
            string file = System.IO.Path.Combine(mainDir, fileName);
            if (!System.IO.File.Exists(file))
            {
                cBase.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            using var fs = System.IO.File.OpenRead(file);
            await fs.CopyToAsync(cBase.Response.Body);
        }
    }
}
