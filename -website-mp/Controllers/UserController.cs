using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _website_mp;
using _website_mp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using _website_mp.Classes;

namespace _website_mp.Controllers
{
    public class UserController : Controller
    {
        public ViewResult Login()
        {
            if (HttpContext.Session.GetString("Account") != null)
            {
                return View("Profile");
            }
            return View();
        }
        
        public ViewResult Logout()
        {
            if (HttpContext.Session.GetString("Account") != null)
            {
                ViewData["msg"] = "Bạn đã đăng xuất thành công";
                HttpContext.Session.Remove("Account");
            }
            return View("Login");
        }

        public ViewResult Signup()
        {
            if (HttpContext.Session.GetString("Account") != null)
            {
                return View("Profile");
            }
            return View();
        }

        public ViewResult Profile()
        {
            string Account = HttpContext.Session.GetString("Account");
            if (Account == null)
            {
                return View("Login");
            }

            var userModel = new UserModel();
            string[] pro = userModel.GetProfileUser(Account);
            ViewData["Account"] = Account;
            ViewData["FullName"] = pro[0];
            ViewData["Email"] = pro[1];
            ViewData["Phone"] = pro[2];
            ViewData["Address"] = pro[3];
            ViewData["City"] = pro[4];
            ViewData["Province"] = pro[5];
            return View();
        }

        public string LoginAccount(string Account, string Password)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var userModel = new UserModel();
            bool ck = userModel.CheckAccount(Account);
            if (ck == false)
                res.Add("AccountError", "Tài khoản không tồn tại");
            else
            {
                if (!userModel.CheckActive(Account))
                    res.Add("AccountError", "Tài khoản đã bị xóa");
                else
                {
                    if (!userModel.CheckPassword(Account, Password))
                    {
                        res.Add("status", "0");
                        res.Add("PasswordError", "Sai mật khẩu");
                    }
                    else
                    {
                        if (userModel.UpdateLastAccess(Account))
                        {
                            res.Add("status", "1");
                            HttpContext.Session.SetString("Account", Account);
                        } else
                        {
                            res.Add("status", "0");
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject(res);
        }

        public string CreateAccount(string Account, string Password, string FullName, 
            string Email, string Phone, string Address, string City, string Province)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            List<string> param = new List<string>
            {
                Account,
                Password,
                FullName,
                Email,
                Phone,
                Address,
                City,
                Province
            };

            var userModel = new UserModel();
            if (userModel.CreateAccount(param) > 0)
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
                if (userModel.CheckAccount(Account))
                {
                    res.Add("AccountError", "Tài khoản đã tồn tại");
                }
                if (userModel.CheckPhone(Phone))
                {
                    res.Add("PhoneError", "Số điện thoại đã được sử dụng cho tài khoản khác");
                }
            }
            return JsonConvert.SerializeObject(res);
        }

        public string UpdatePassword(string Account, string Password, string NewPassword)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var userModel = new UserModel();
            if (!userModel.CheckPassword(Account, Password))
            {
                res.Add("status", "0");
                res.Add("PasswordError", "Sai mật khẩu!!");
            }
            else
            {
                if (userModel.UpdatePassword(Account, NewPassword))
                {
                    res.Add("status", "1");
                }
                else
                {
                    res.Add("status", "0");
                }
            }
            return JsonConvert.SerializeObject(res);
        }

        public string GetImgUser()
        {
            string Account = HttpContext.Session.GetString("Account");
            var userModel = new UserModel();
            byte[] i = userModel.GetImg(Account);
            if (i != null)
            {
                return JsonConvert.ToString(BitConverter.ToString(i));
            }
            else
            {
                return JsonConvert.ToString("none");
            }
        }

        public string SetImgUser(string Account, string Img)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var userModel = new UserModel();
            if (userModel.SetImg(Functions.StringToByteArray(Img), Account))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string UpdateUser(string FullName, string Email, string Address, string City, string Province)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            string Account = HttpContext.Session.GetString("Account");
            var userModel = new UserModel();
            if (userModel.UpdateAccount(Account, FullName, Email, Address, City, Province))
            {
                res.Add("status", "1");
            }
            else
            {
                res.Add("status", "0");
            }
            return JsonConvert.SerializeObject(res);
        }

        public string DeleteAccount()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            string Account = HttpContext.Session.GetString("Account");
            var userModel = new UserModel();
            if (userModel.DeleteAccount(Account))
            {
                HttpContext.Session.Remove("Account");
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
