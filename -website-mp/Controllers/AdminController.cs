using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _website_mp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace _website_mp.Controllers
{
    public class AdminController : Controller
    {
        public ViewResult Index()
        {
            if(HttpContext.Session.GetString("Admin") != null)
            {
                var orderModel = new OrderModel();
                ViewData["earn-month"] = orderModel.EarnMonth();
                ViewData["earn-anual"] = orderModel.EarnYear();
                ViewData["pending"] = orderModel.Pending();
                ViewData["shipping"] = orderModel.Shipping();
                return View("Index");
            }
            else
            {
                return View("Login");
            }
        }
        
        public ViewResult Login(string Password)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            if (Password == configuration["PasswordAdmin"])
            {
                HttpContext.Session.SetString("Admin", Password);
                return Index();
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            return Index();
        }

        public ViewResult Info_User()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/User/Info.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Info_Warehouse()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Warehouse/Info.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Insert_Warehouse()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Warehouse/Insert.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Info_Type_Product()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Product/Type/Info.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Insert_Type_Product()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Product/Type/Insert.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Info_Product()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Product/Info.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Insert_Product()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Product/Insert.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Info_Order()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Order/Info.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Order_Cancel()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Order/OrderCancel.cshtml");
            }
            else
            {
                return View("Login");
            }
        }

        public ViewResult Info_Invoice()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return View("/Views/Admin/Order/Invoice/Info.cshtml");
            }
            else
            {
                return View("Login");
            }
        }
    }
}
