using _website_mp.Classes;
using _website_mp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Controllers
{
    public class ProductController : Controller
    {
        public string GetAllProduct()
        {
            var producModel = new ProductModel();
            string[][] arr = producModel.GetAll();
            List<string> res = new List<string>();
            foreach(string[] val in arr)
            {
                if (Convert.ToInt32(val[10]) <= 0) continue;
                string row = "<div class='product col pb-2 pl-0 pr-0' show='show'>";
                row += "<div class='border-hover d-flex align-items-end flex-column m-1 h-100'>";
                row += "<div class='hovereffect'>";
                row += "<a href='/product/infoProduct/"+val[0]+"'>";
                row += "<img id='img' class='img-fluid img_p' src='' id_p='"+val[0]+"'>";
                row += "</a></div>";
                row += "<div class='mt-auto'><p class='m-2'><a class='name-product' href='";
                row += "/product/infoProduct/"+val[0]+"'>";
                row +=  val[1];
                row += "</a></p><p class='price-product m-2'>";
                row += Functions.FormatPrice(val[4]);
                row += "₫</p></div></div></div>";
                res.Add(row);
            }
            return JsonConvert.SerializeObject(res.ToArray());
        }

        public ViewResult GetByType(string id)
        {
            var producModel = new ProductModel();
            string[][] arr = producModel.GetByType(id);
            List<string> res = new List<string>();
            foreach (string[] val in arr)
            {
                if (Convert.ToInt32(val[10]) <= 0) continue;
                string row = "<div class='product col pb-2 pl-0 pr-0' show='show'>";
                row += "<div class='border-hover d-flex align-items-end flex-column m-1 h-100'>";
                row += "<div class='hovereffect'>";
                row += "<a href='/product/infoProduct/" + val[0] + "'>";
                row += "<img id='img' class='img-fluid img_p' src='' id_p='" + val[0] + "'>";
                row += "</a></div>";
                row += "<div class='mt-auto'><p class='m-2'><a class='name-product' href='";
                row += "/product/infoProduct/" + val[0] + "'>";
                row += val[1];
                row += "</a></p><p class='price-product m-2'>";
                row += Functions.FormatPrice(val[4]);
                row += "₫</p></div></div></div>";
                res.Add(row);
            }
            ViewBag.arr = res.ToArray();
            return View();
        }

        public string GetImageProduct(string id)
        {
            var producModel = new ProductModel();
            byte[] i = producModel.GetImgProduct(id);
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("id", id);
            if (i != null)
            {
                res.Add("img", BitConverter.ToString(i));
            }
            else
            {
                res.Add("img", "none");
            }
            return JsonConvert.SerializeObject(res);
        }

        public ViewResult InfoProduct(string id)
        {
            var producModel = new ProductModel();
            string[] p = producModel.GetOne(id);
            ViewData["id"] = p[0];
            ViewData["name"] = p[1];
            ViewData["brand"] = p[2];
            ViewData["color"] = p[3];
            ViewData["price"] = Functions.FormatPrice(p[4]);
            ViewData["short-discription"] = p[6];
            ViewData["discription"] = p[7];
            return View("InfoProduct");
        }

        public string AddCart(string account, string id_p, string quan)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var productModel = new ProductModel();
            if (productModel.AddCart(account, id_p, quan))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }
    }
}
