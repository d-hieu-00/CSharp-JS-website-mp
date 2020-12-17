using _website_mp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Models
{
    public class CartModel
    {
        private Database Db { get; set; }
        public CartModel()
        {
            Db = new Database();
        }

        public string[][] GetCartInfo(string account)
        {
            string query = "select cd.id_product id, cd.quantity quan from (mp_cart_detail cd " +
            " join mp_cart c on cd.id_cart = c.id) join mp_user u on u.id = c.id_user where " +
            "u.account =@p1 ";
            List<string> param = new List<string>
            {
                account
            };
            return Db.QuerySelect(query, 2, param).ToArray();
        }

        public string GetIdCart(string account)
        {
            string query = "select c.id id from mp_user u join mp_cart c " +
            " on u.id = c.id_user where u.account =@p1";
            List<string> param = new List<string>
            {
                account
            };
            return Db.QuerySelect(query, 1, param).ToArray()[0][0];
        }

        public bool RemoveCartDetail(string id_c, string id_p)
        {
            string query = "delete from mp_cart_detail where id_cart=@p1 and id_product=@p2";
            List<string> param = new List<string>
            {
                id_c,
                id_p
            };
            return Db.Query(query, param) > 0;
        }

        public bool UpdateCartDetail(string quan, string id_c, string id_p)
        {
            string query = "update mp_cart_detail set quantity=@p1 where id_cart=@p2 and id_product=@p3";
            List<string> param = new List<string>
            {
                quan,
                id_c,
                id_p
            };
            return Db.Query(query, param) > 0;
        }

        public bool RemoveAllCartDetail(string id_c)
        {
            string query = "delete from mp_cart_detail where id_cart=@p1";
            List<string> param = new List<string>
            {
                id_c
            };
            return Db.Query(query, param) > 0;
        }
    }
}
