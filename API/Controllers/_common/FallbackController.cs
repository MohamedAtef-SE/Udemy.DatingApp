﻿using Microsoft.AspNetCore.Mvc;

namespace API.Controllers._common
{
    public class FallbackController : Controller
    {
        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory()
                , "wwwroot", "index.html"), "text/HTML");
        }
    }
}
