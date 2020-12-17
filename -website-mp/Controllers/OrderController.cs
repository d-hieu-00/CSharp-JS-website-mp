﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _website_mp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace _website_mp.Controllers
{
    public class OrderController : Controller
    {
        public ViewResult Info()
        {
            return View();
        }
        /*public function order(){
            $res = array();
            $Account = $this->input('Account');
            $FullName = $this->input('FullName');
            $Phone = $this->input('Phone');
            $Address = $this->input('Address');
            $City = $this->input('City');
            $Province = $this->input('Province');
            $Total = $this->input('Total');
            $Cd = $this->input('Cd');

            $orderModel = $this->model('orderModel');
            $r = $orderModel->order($FullName, $Phone, $Address, $City, $Province, $Total, $Cd, $Account);
            if($r == true){
                $res['status'] = true;
            } else {
                $res['status'] = false;
            }
            echo json_encode($res);
        }*/

        public string Order(string Account, string FullName, string Phone, string Address,
            string City, string Province, string Total, string[][] Cd)
        {
            var orderModel = new OrderModel();
            Dictionary<string, string> res = new Dictionary<string, string>();
            string Id_user = null;
            if (Account != null)
            {
                var userModel = new UserModel();
                Id_user = userModel.GetIdUser(Account);
            }
            if (orderModel.Order(FullName, Phone, Address, City, Province, Total, Id_user, Cd))
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
