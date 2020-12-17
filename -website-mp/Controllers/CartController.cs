using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _website_mp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _website_mp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Info()
        {
            return View();
        }

        public string GetInfoByIdProduct(string id_p, string quan)
        {
            var productModel = new ProductModel();
            string[] p = productModel.GetOne(id_p);
            Dictionary<string, string> pro = new Dictionary<string, string>
            {
                { "id", p[0] },
                { "name", p[1] },
                { "price", p[4] }
            };
            Dictionary<string, string> res = new Dictionary<string, string>
            {
                { "quan", quan },
                { "p", JsonConvert.SerializeObject(pro) }
            };
            return JsonConvert.SerializeObject(res);
        }

        public string GetCartInfo()
        {
            string account = HttpContext.Session.GetString("Account");
            var cartModel = new CartModel();
            string[][] c = cartModel.GetCartInfo(account);

            List<string> res = new List<string>();
            

            foreach (string[] val in c)
            {
                Dictionary<string, string> row = new Dictionary<string, string>()
                {
                    { "id", val[0] },
                    { "quan", val[1] }
                };
                res.Add(JsonConvert.SerializeObject(row));
            }
            return JsonConvert.SerializeObject(res);
        }

        public string UpdateQuantity(string id_p, string quan)
        {
            var cartModel = new CartModel();
            string account = HttpContext.Session.GetString("Account");
            string id_c = cartModel.GetIdCart(account);
            Dictionary<string, string> res = new Dictionary<string, string>();
            if (cartModel.UpdateCartDetail(quan, id_c, id_p))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string RemoveCartDetail(string id_p)
        {
            var cartModel = new CartModel();
            string account = HttpContext.Session.GetString("Account");
            string id_c = cartModel.GetIdCart(account);
            Dictionary<string, string> res = new Dictionary<string, string>();
            if(cartModel.RemoveCartDetail(id_c, id_p))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }
        public string RemoveAllCartDetail()
        {
            var cartModel = new CartModel();
            string account = HttpContext.Session.GetString("Account");
            string id_c = cartModel.GetIdCart(account);
            Dictionary<string, string> res = new Dictionary<string, string>();
            if (cartModel.RemoveAllCartDetail(id_c))
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
