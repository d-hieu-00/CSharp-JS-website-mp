using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace _website_mp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Admin/_assets/Index.cshtml");
        }
    }
}
