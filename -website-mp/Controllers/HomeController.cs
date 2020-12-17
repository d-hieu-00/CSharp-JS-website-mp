using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.Extensions.Logging;
using _website_mp.Models;
using Newtonsoft.Json;

namespace _website_mp.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
        public string getCategory()
        {
            TypeProductModel model = new TypeProductModel();
            List<string[][]> rs = new List<string[][]>();
            for (int i = 1; i < 5; i++)
            {
                string[][] r = model.GetTypeProductByIdCategory(Convert.ToString(i));
                rs.Add(r);
            }
            return JsonConvert.SerializeObject(rs.ToArray());
        }
    }
}
