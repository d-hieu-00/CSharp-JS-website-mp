using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _website_mp.Classes;
using _website_mp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
                ViewData["msg"] = "Sai mật khẩu";
                return View("Login");
            }
        }

        public ViewResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            return Index();
        }
        /**
         * 
         * User
         * 
         */
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

        public string GetAllUser()
        {
            var userModel = new UserModel();
            string[][] AllUser = userModel.GetAll();
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach(string[] val in AllUser)
            {
                // id 0, account 1, password 2, fullname 3, email 4, phone 5, address 6, city 7,
                // province 8, status 9, usercreatedDate 10, passwordmodifyDate 11, usermodifydate 12,
                // userlastaccess 13
                List<string> r = new List<string>();
                r.Add("<span>"+(i++)+"</span>");
                r.Add("<span>"+val[1]+"</span>");
                r.Add("<span>"+val[3]+"</span>");
                r.Add("<span>"+val[4]+"</span>");
                r.Add("<span>"+val[5]+"</span>");
                r.Add("<span>"+val[7]+"</span>");
                if(val[9] == "ACTIVE")
                {
                    r.Add("<button class='btn status btn-success' account='" + val[1] + "'>" +
                        val[9] + "</button>");
                }
                else
                {
                    r.Add("<button class='btn status btn-danger' account='" + val[1] + "'>" +
                        val[9] + "</button>");
                }
                r.Add("<span>" + val[8] + "</span>");
                r.Add("<span>" + val[6] + "</span>");
                r.Add("<span>" + val[10] + "</span>");
                r.Add("<span>" + val[13] + "</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }

        public string DisableUser(string Account)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var userModel = new UserModel();
            if (userModel.DeleteAccount(Account))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string ActiveUser(string Account)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var userModel = new UserModel();
            if (userModel.AcitveAccount(Account))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }
        /**
         * 
         * 
         * Warehouse
         * 
         */
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

        public string GetAllWarehouse()
        {
            var warehouseModel = new WarehouseModel();
            string[][] AllWarehouse = warehouseModel.GetAll();
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach (string[] val in AllWarehouse)
            {
                // id 0, name 1, address 2, city 3, province 4, status 5, date_created 6, date_modify 7
                List<string> r = new List<string>();
                r.Add("<span>" + (i++) + "</span>");
                r.Add("<span class='w-name' id_w='"+val[0]+"'>" + val[1] + "</span>");
                r.Add("<span class='w-city' id_w='"+val[0]+"'>" + val[3] + "</span>");
                r.Add("<span class='w-province' id_w='"+val[0]+"'>" + val[4] + "</span>");
                r.Add("<span class='w-address' id_w='"+val[0]+"'>" + val[2] + "</span>");

                if (val[5] == "ACTIVE")
                {
                    r.Add("<button class='btn status btn-success' id_w='" + val[0] + "'>" +
                        val[5] + "</button>");
                    r.Add("<button class='btn modify btn-info' id_w='" + val[0] + "'" +
                        "data-toggle='modal' data-target='#modify-warehouse'>Sửa</button>");
                    r.Add("<button class='btn detail btn-info' id_w='" + val[0] + "'" +
                        "data-toggle='modal' data-target='#detail-warehouse'>Chi tiết</button>");
                }
                else
                {
                    r.Add("<button class='btn status btn-danger' id_w='" + val[0] + "'>" +
                        val[5] + "</button>");
                    r.Add("<button class='btn modify btn-info' id_w='" + val[0] + "'" +
                        "data-toggle='modal' data-target='#modify-warehouse'disabled>Sửa</button>");
                    r.Add("<button class='btn detail btn-info' id_w='" + val[0] + "'" +
                        "data-toggle='modal' data-target='#detail-warehouse' disabled>Chi tiết</button>");
                }
                r.Add("<span>" + val[6] + "</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }

        public string DisableWarehouse(string id)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var warehouseModel = new WarehouseModel();
            if (warehouseModel.DisableWarehouse(id))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string ActiveWarehouse(string id)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var warehouseModel = new WarehouseModel();
            if (warehouseModel.ActiveWarehouse(id))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string SaveWarehouse(string name, string city, string province, string address, string id)
        {
            var warehouseModel = new WarehouseModel();
            Dictionary<string, string> res = new Dictionary<string, string>();

            if (warehouseModel.Save(id, name, address, city, province))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string InsertWarehouse(string name, string city, string province, string address, string status)
        {
            var warehouseModel = new WarehouseModel();
            Dictionary<string, string> res = new Dictionary<string, string>();

            if(warehouseModel.Insert(name, city, province, address, status))
            {
                res.Add("status", "1");
            }
            else if (warehouseModel.CheckName(name))
            {
                res.Add("status", "0");
                res.Add("NameError", "Kho đã tồn tại");
            } 
            else
            {
                res.Add("msg", "Lỗi không xác định!!");
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }
        public string DetailsWarehouse(string id)
        {
            var warehouseModel = new WarehouseModel();
            string[][] AllWarehouse = warehouseModel.Details(id);
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach (string[] val in AllWarehouse)
            {
                // p.name 0, p.brand 1, wd.quantity 2, wd.date_created 3, wd.date_modify 4
                List<string> r = new List<string>();
                r.Add("<span>"+(i++)+"</span>");
                r.Add("<span>"+val[0]+"</span>");
                r.Add("<span>"+val[1]+"</span>");
                r.Add("<span>"+val[2]+"</span>");
                r.Add("<span>"+val[4]+"</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }
        /**
         * 
         * 
         * Type product
         * 
         */
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

        public string GetAllTypeProduct()
        {
            var tpModel = new TypeProductModel();
            string[][] allTP = tpModel.GetAll();
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach (string[] val in allTP)
            {
                // tp.id 0, tp.name 1, count(p.id_type) quantity 2, c.id id_category 3,
                // c.name name_category 4, tp.status 5, tp.date_created 6
                List<string> r = new List<string>();
                r.Add("<span>" + (i++) + "</span>");
                r.Add("<span class='tp-name' id_tp='" + val[0] + "'>" + val[1] +"</span>");
                r.Add("<span class='tp-quantity' id_tp='" + val[0] + "'>" + val[2] +"</span>");
                r.Add("<span class='tp-category' id_tp='" + val[0] + "' id_category='" + val[3]
                    + "'>" + val[4] + "</span>");
                if(val[5] == "ACTIVE")
                {
                    r.Add("<button class='btn status btn-success' id_tp='" + val[0] + "'>" + 
                          val[5] + "</button>");
                    r.Add("<button class='btn modify btn-info' id_tp='" + val[0] + "'" +
                          "data-toggle = 'modal' data-target = '#modify-type-product' > Sửa </button>");
                } 
                else
                {
                    r.Add("<button class='btn status btn-danger' id_tp='" + val[0] + "'>" +
                          val[5] + "</button>");
                    r.Add("<button class='btn modify btn-info' id_tp='" + val[0] + "'" +
                          "data-toggle = 'modal' data-target = '#modify-type-product' disabled> Sửa </button>");
                }
                r.Add("<span>" + val[6] +"</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }

        public string DisableTypeProduct(string id)
        {
            var tpModel = new TypeProductModel();
            Dictionary<string, string> res = new Dictionary<string, string>();

            if (tpModel.DisableTypeProduct(id))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string ActiveTypeProduct(string id)
        {
            var tpModel = new TypeProductModel();
            Dictionary<string, string> res = new Dictionary<string, string>();

            if (tpModel.ActiveTypeProduct(id))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string SaveTypeProduct(string id, string name, string id_category)
        {
            var tpModel = new TypeProductModel();
            Dictionary<string, string> res = new Dictionary<string, string>();

            if (tpModel.Save(id, id_category, name))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string InsertTypeProduct(string name, string id_category, string status)
        {
            var tpModel = new TypeProductModel();
            Dictionary<string, string> res = new Dictionary<string, string>();

            if (tpModel.Insert(name, id_category, status))
            {
                res.Add("status", "1");
            }
            else if (tpModel.CheckName(name))
            {
                res.Add("NameError", "Tên kho không được trùng!!");
                res.Add("status", "0");
            }
            else
            {
                res.Add("msg", "Lỗi không xác định!!");
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }
        /**
         * 
         * 
         * Product
         * 
         */
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

        public string GetAllProduct()
        {
            var productModel = new ProductModel();
            string[][] allProduct = productModel.GetAll();
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach (string[] val in allProduct)
            {
                // mp.id 0, mp.name 1, mp.brand 2, mp.color 3, mp.price 4, mp.img 5, mp.short_discription 6, mp.discription 7,
                // tp.id type_id 8, tp.name type_name 9, sum(wd.quantity) quantity 10, 
                // mp.status 11, mp.date_created 12, mp.date_modify 13
                List<string> r = new List<string>();
                r.Add("<span>" + (i++) + "</span>");
                r.Add("<span class='p-name' id_p='" + val[0] + "'>" + val[1] + "</span>");
                r.Add("<span class='p-type-name' id_p='" + val[0] + "' type_id='" + val[8] + "'>" +
                    val[9] + "</span>");
                r.Add("<span class='p-brand' id_p='" + val[0] + "'>" + val[2] + "</span>");
                if(val[3] == null)
                {
                    r.Add("<span class='p-color'>none</span>");
                }
                else
                {
                    r.Add("<input disabled type='color' class='form-control p-color' " + 
                     " id_p = '" + val[0] + "'value = '" + val[3] + "' > ");
                }
                r.Add("<span class='p-price' id_p='" + val[0] + "'>" + val[4] + "</span>");
                r.Add("<span class='p-quantity' id_p='" + val[0] + "'>" + val[10] + "</span>");
                if (val[11] == "ACTIVE")
                {
                    r.Add("<button class='btn status btn-success' id_p='" + val[0] + "'>" +
                          val[11] + "</button>");
                    r.Add("<button class='btn modify btn-info' id_p='" + val[0] + "'" +
                          "data-toggle = 'modal' data-target = '#modify-product' > Sửa </button>");
                }
                else
                {
                    r.Add("<button class='btn status btn-danger' id_p='" + val[0] + "'>" +
                          val[11] + "</button>");
                    r.Add("<button class='btn modify btn-info' id_p='" + val[0] + "'" +
                          "data-toggle = 'modal' data-target = '#modify-product' disabled> Sửa </button>");
                }
                r.Add("<span class='p-short-discription' id_p='" + val[0] + "'>" + val[6] + "</span>");
                r.Add("<span class='p-discription' id_p='" + val[0] + "'>" + val[7] + "</span>");
                r.Add("<span>" + val[12] + "</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }

        public string DisableProduct(string id)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var productModel = new ProductModel();
            if (productModel.DisableProduct(id))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string ActiveProduct(string id)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var productModel = new ProductModel();
            if (productModel.ActiveProduct(id))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string GetTypeProductForTagSelect()
        {
            // tp.id 0, tp.name 1, count(p.id_type) quantity 2, c.id id_category 3
            // c.name name_category 4, tp.status 5, tp.date_created 6
            Dictionary<string, string> res = new Dictionary<string, string>();
            var tpModel = new TypeProductModel();
            string[][] allTP = tpModel.GetAll();
            List<string> data = new List<string>();
            foreach(string[] row in allTP)
            {
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("id", row[0]);
                r.Add("name", row[1]);
                data.Add(JsonConvert.SerializeObject(r));
            }
            res.Add("status", "1");
            res.Add("data", JsonConvert.SerializeObject(data.ToArray()));
            return JsonConvert.SerializeObject(res);
        }

        public string GetWarehouseForTagSelect()
        {
            // id 0, name 1, address 2, city 3, province 4, status 5, date_created 6, date_modify 7
            Dictionary<string, string> res = new Dictionary<string, string>();
            var warehouseModel = new WarehouseModel();
            string[][] allWH = warehouseModel.GetAll();
            List<string> data = new List<string>();
            foreach (string[] row in allWH)
            {
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("id", row[0]);
                r.Add("name", row[1]);
                data.Add(JsonConvert.SerializeObject(r));
            }
            res.Add("status", "1");
            res.Add("data", JsonConvert.SerializeObject(data.ToArray()));
            return JsonConvert.SerializeObject(res);
        }

        public string GetOneProduct(string id)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var productModel = new ProductModel();
            string[] pro = productModel.Get(id);
            byte[] img = productModel.GetImgProduct(id);
            //`id` 0, `name`1, `brand`2, `color`3, `price`4, `img`5, `short_discription`6, 
            //`discription`7, `id_type`8, `date_created`9, `status`10, `date_modify`11
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "id", pro[0] },
                { "name", pro[1] },
                { "brand", pro[2] },
                { "color", pro[3] },
                { "price", pro[4] },
                { "short_discription", pro[6] },
                { "discription", pro[7] },
                { "id_type", pro[8] },
                { "date_created", pro[9] },
                { "status", pro[10] },
                { "date_modify", pro[11] },
                { "img", Encoding.ASCII.GetString(img) }
            };
            res.Add("status", "1");
            res.Add("data", JsonConvert.SerializeObject(data));
            return JsonConvert.SerializeObject(res);
        }

        public string GetWarehouseByIdProduct(string id)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var productModel = new ProductModel();
            string[][] warehouse = productModel.GetWarehouse(id);
            // wd.id_warehouse 0, wd.quantity 1
            List<string> data = new List<string>();
            foreach (string[] row in warehouse)
            {
                Dictionary<string, string> r = new Dictionary<string, string>();
                r.Add("id_warehouse", row[0]);
                r.Add("quantity", row[1]);
                data.Add(JsonConvert.SerializeObject(r));
            }
            res.Add("status", "1");
            res.Add("data", JsonConvert.SerializeObject(data.ToArray()));
            return JsonConvert.SerializeObject(res);
        }

        public string SaveProduct(string id, string name, string brand, string color, string price, string img,
            string short_discription, string discription, string id_type, string[][] warehouse)
        {
            var productModel = new ProductModel();
            var warehouseModel = new WarehouseModel();
            Dictionary<string, string> res = new Dictionary<string, string>();
            productModel.Save(name, brand, color, price, Functions.StringToByteArray(img), 
                short_discription, discription, id_type, id);
            warehouseModel.DeleteAllWarehouseDetails(id);
            foreach(string[] val in warehouse)
            {
                warehouseModel.InsertWarehouseDetail(val[0], val[1], val[2]);
            }
            res.Add("status", "1");
            return JsonConvert.SerializeObject(res);
        }

        public string InsertProduct(string name, string brand, string color, string price, string img,
            string short_discription, string discription, string id_type, string[][] warehouse)
        {
            var productModel = new ProductModel();
            var warehouseModel = new WarehouseModel();
            Dictionary<string, string> res = new Dictionary<string, string>();
            string id = productModel.Insert(name, brand, color, price, Functions.StringToByteArray(img),
                short_discription, discription, id_type);
            if(id == null)
            {
                res.Add("status", "0");
                return JsonConvert.SerializeObject(res);
            }
            foreach (string[] val in warehouse)
            {
                warehouseModel.InsertWarehouseDetail(val[0], id, val[1]);
            }
            res.Add("status", "1");
            return JsonConvert.SerializeObject(res);
        }
        /**
         * 
         * 
         * Order
         * 
         */
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

        public string GetAllOrder()
        {
            var orderModel = new OrderModel();
            string[][] allOD = orderModel.GetAllOrder();
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach (string[] val in allOD)
            {
                // select o.id 0, o.full_name 1, o.phone 2, o.shipping_fee 3, o.total_price 4, o.status 5, 
                // CONCAT(o.address, ', ', o.province, ', ', o.city) address 6, u.account 7, o.date_created 8, o.date_modify 9
                List<string> r = new List<string>();
                r.Add("<span>" + (i++) + "</span>");
                r.Add("<span class='o-full-name'>" + val[1] + "</span>");
                r.Add("<span class='o-phone'>" + val[2] + "</span>");
                r.Add("<span class='o-shipping-fee'>" + Functions.FormatPrice(val[3]) + "</span>");
                r.Add("<span class='o-total-price'>" + Functions.FormatPrice(val[4]) + "</span>");
                if(val[5] == "Chờ xác nhận")
                {
                    r.Add("<span class='btn btn-secondary o-status'>" + val[5] + "</span>");
                    r.Add("<button class='btn btn-info modify' id_o='"+val[0]+
                        "' data-toggle='modal' data-target='#modify-order'> Xác nhận </button>");
                    r.Add("<button class='btn btn-primary next-status' id_o='" + val[0] +
                        "' disabled> Chuyển </button>");
                } 
                else if(val[5] == "Đã xác nhận")
                {
                    r.Add("<span class='btn btn-success o-status'>" + val[5] + "</span>");
                    r.Add("<button class='btn btn-info detail' id_o='" + val[0] +
                        "' data-toggle='modal' data-target='#detail-order'> Chi tiết đơn hàng </button>");
                    r.Add("<button class='btn btn-primary next-status' id_o='" + val[0] +
                        "'> Chuyển </button>");
                }
                else
                {
                    r.Add("<span class='btn btn-warning o-status'>" + val[5] + "</span>");
                    r.Add("<button class='btn btn-info detail' id_o='" + val[0] +
                        "' data-toggle='modal' data-target='#detail-order'> Chi tiết đơn hàng </button>");
                    r.Add("<button class='btn btn-primary next-status' id_o='" + val[0] +
                        "'> Chuyển </button>");
                }
                r.Add("<button class='btn o-cancel btn-danger' id_o='"  + val[0] + "'>Hủy đơn hàng</button>");
                r.Add("<span class='o-address' id_o='" + val[0] + "'>" + val[6] + "</span>");
                if(val[7] == "")
                {
                    r.Add("<span class='o-account' id_o='" + val[0] + "'><i>(none)</i></span>");
                }
                else
                {
                    r.Add("<span class='o-account' id_o='" + val[0] + "'>" + val[7] + "</span>");
                }
                r.Add("<span>" + val[8] + "</span>");
                r.Add("<span>" + val[9] + "</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }

        public string DetailOrder(string id_o)
        {
            var orderModel = new OrderModel();
            var warehouseModel = new WarehouseModel();
            Dictionary<string, string> res = new Dictionary<string, string>();
            // o.id 0, o.full_name 1, o.phone 2, o.shipping_fee 3, o.address 4, o.province 5, o.city 6
            string[] order = orderModel.GetDetailOrder(id_o);
            Dictionary<string, string> _order = new Dictionary<string, string>()
            {
                { "id", order[0] },
                { "full_name", order[1] },
                { "phone", order[2] },
                { "shipping_fee", order[3] },
                { "address", order[4] },
                { "province", order[5] },
                { "city", order[6] }
            };
            res.Add("order", JsonConvert.SerializeObject(_order));

            // od.id_product 0, p.name 1, od.quantity 2, od.id_warehouse 3, od.price 4
            string[][] od = orderModel.GetDetailProductOrder(id_o);
            List<string> _od = new List<string>();
            List<string> _w = new List<string>();
            foreach (string[] val in od)
            {
                Dictionary<string, string> r_od = new Dictionary<string, string>
                {
                    { "id_product", val[0] },
                    { "name", val[1] },
                    { "quantity", val[2] },
                    { "id_warehouse", val[3] },
                    { "price", val[4] }
                };
                _od.Add(JsonConvert.SerializeObject(r_od));
                // w.id 0, w.name 1, wd.quantity 2, concat(w.name,' - ',wd.quantity) display 3
                string[][] w = warehouseModel.DetailProduct(val[0]);
                List<string> __w = new List<string>();
                foreach(string[] v in w)
                {
                    Dictionary<string, string> r_w = new Dictionary<string, string>
                    {
                        { "id", v[0] },
                        { "name", v[1] },
                        { "quantity", v[2] },
                        { "display", v[3] }
                    };
                    __w.Add(JsonConvert.SerializeObject(r_w));
                }
                _w.Add(JsonConvert.SerializeObject(__w.ToArray()));
            }
            res.Add("od", JsonConvert.SerializeObject(_od));
            res.Add("w", JsonConvert.SerializeObject(_w));
            return JsonConvert.SerializeObject(res);
        }

        public string ConfirmOrder(string[] o, string[][] od)
        {
            var orderModel = new OrderModel();
            var warehouseModel = new WarehouseModel();
            Dictionary<string, string> res = new Dictionary<string, string>();
            // wd.id_p 0, wd.quan 1, wd.price 2, wd.id_w 3, wd.quan_w 4
            // o.id 0, o.full_name 1, o.shipping_fee 2, o.city 3, o.province 4, o.address 5, o.total_price 6
            orderModel.UpdateOrder(o[2], o[6], o[1], o[5], o[3], o[4], o[0]);
            foreach(string[] val in od)
            {
                orderModel.UpdateDetailOrder(val[3], val[1], val[2], o[0], val[0]);
                warehouseModel.UpdateWarehouseDetail(val[4], val[0], val[3]);
            }
            orderModel.UpdateStatusCofirmed(o[0]);
            res.Add("status", "1");
            return JsonConvert.SerializeObject(res);
        }

        public string NextStatus(string id)
        {
            var orderModel = new OrderModel();
            Dictionary<string, string> res = new Dictionary<string, string>();
            string status = orderModel.GetStatus(id);
            if (status == "Đã xác nhận")
            {
                orderModel.UpdateStatusDeliver(id);
            }
            else
            {
                orderModel.UpdateStatusDone(id);
            }
            res.Add("status", "1");
            return JsonConvert.SerializeObject(res);
        }
            //    public function cancel()
            //    {
            //    $res = array();
            //    $orderModel = $this->model('orderModel');
            //    $id = $this->input('id');
            //    $orderModel->updateStatusCancel($id);
            //    $status = $orderModel->getStatus($id)->status;
            //        if ($status != "Chờ xác nhận"){
            //        $od = $orderModel->getDetailProductOrder($id);
            //        $warehouseModel = $this->model('warehouseModel');
            //            foreach ($od as $val) {
            //            $data = [
            //                $val->quantity,
            //                $val->id_product,
            //                $val->id_warehouse
            //            ];
            //            $warehouseModel->restoreWarehouse($data);
            //            }
            //        }
            //    $res['status'] = true;
            //        echo json_encode($res);
            //    }
            //    // order have cancel
            //    public function order_cancel()
            //    {
            //        if ($this->getSession('Admin')) {
            //        $this->view("admin/order", "orderCancel");
            //    } else {
            //        $this->redirect('admin/login');
            //    }
            //}
        public string Cancel(string id)
        {
            var orderModel = new OrderModel();
            Dictionary<string, string> res = new Dictionary<string, string>();
            string status = orderModel.GetStatus(id);
            if(status != "Chờ xác nhận")
            {
                // od.id_product 0, p.name 1, od.quantity 2, od.id_warehouse 3, od.price 4
                string[][] od = orderModel.GetDetailProductOrder(id);
                var warehouseModel = new WarehouseModel();
                foreach(string[] val in od)
                {
                    warehouseModel.RestoreWarehouse(val[2], val[0], val[3]);
                }
            }
            orderModel.UpdateStatusCancel(id);
            res.Add("status", "1");
            return JsonConvert.SerializeObject(res);
        }

        public string GetAllOrderCancel()
        {
            var orderModel = new OrderModel();
            string[][] allODC = orderModel.GetAllOrderCancel();
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach (string[] val in allODC)
            {
                // select o.id 0, o.full_name 1, o.phone 2, o.shipping_fee 3, o.total_price 4, o.status 5, 
                // CONCAT(o.address, ', ', o.province, ', ', o.city) address 6, u.account 7, o.date_created 8, o.date_modify 9
                List<string> r = new List<string>();
                r.Add("<span>" + (i++) + "</span>");
                r.Add("<span class='o-full-name'>" + val[1] + "</span>");
                r.Add("<span class='o-phone'>" + val[2] + "</span>");
                r.Add("<span class='o-shipping-fee'>" + Functions.FormatPrice(val[3]) + "</span>");
                r.Add("<span class='o-total-price'>" + Functions.FormatPrice(val[4]) + "</span>");
                r.Add("<button class='btn btn-info detail' id_o='" + val[0] +
                        "' data-toggle='modal' data-target='#detail-order'> Chi tiết đơn hàng hủy</button>");
                if (val[7] == "")
                {
                    r.Add("<span class='o-account' id_o='" + val[0] + "'><i>(none)</i></span>");
                }
                else
                {
                    r.Add("<span class='o-account' id_o='" + val[0] + "'>" + val[7] + "</span>");
                }
                r.Add("<span class='o-address' id_o='" + val[0] + "'>" + val[6] + "</span>");
                r.Add("<span>" + val[8] + "</span>");
                r.Add("<span>" + val[9] + "</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }

        public string GetAllInvoice()
        {
            var orderModel = new OrderModel();
            string[][] allInvoice = orderModel.GetAllInvoice();
            Dictionary<string, string[][]> res = new Dictionary<string, string[][]>();
            List<string[]> rs = new List<string[]>();
            int i = 1;
            foreach (string[] val in allInvoice)
            {
                // select o.id 0, o.full_name 1, o.phone 2, o.shipping_fee 3, o.total_price 4, o.status 5, 
                // CONCAT(o.address, ', ', o.province, ', ', o.city) address 6, u.account 7, o.date_created 8, o.date_modify 9
                List<string> r = new List<string>();
                r.Add("<span>" + (i++) + "</span>");
                r.Add("<span class='o-full-name'>" + val[1] + "</span>");
                r.Add("<span class='o-phone'>" + val[2] + "</span>");
                r.Add("<span class='o-shipping-fee'>" + Functions.FormatPrice(val[3]) + "</span>");
                r.Add("<span class='o-total-price'>" + Functions.FormatPrice(val[4]) + "</span>");
                r.Add("<button class='btn btn-info detail' id_o='" + val[0] +
                        "' data-toggle='modal' data-target='#detail-order'> Chi tiết hóa đơn</button>");
                if (val[7] == "")
                {
                    r.Add("<span class='o-account' id_o='" + val[0] + "'><i>(none)</i></span>");
                }
                else
                {
                    r.Add("<span class='o-account' id_o='" + val[0] + "'>" + val[7] + "</span>");
                }
                r.Add("<span class='o-address' id_o='" + val[0] + "'>" + val[6] + "</span>");
                r.Add("<span>" + val[8] + "</span>");
                r.Add("<span>" + val[9] + "</span>");
                rs.Add(r.ToArray());
            }
            res.Add("data", rs.ToArray());
            return JsonConvert.SerializeObject(res);
        }
    }
}
