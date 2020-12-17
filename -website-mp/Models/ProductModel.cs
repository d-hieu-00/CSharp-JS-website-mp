using _website_mp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Models
{
    public class ProductModel
    {
        private Database Db { get; set; }
        public ProductModel()
        {
            Db = new Database();
        }
        public string[][] GetAll()
        {
            string query = "select mp.id, mp.name, mp.brand, mp.color, mp.price, mp.img, mp.short_discription, mp.discription,"+
                " tp.id type_id, tp.name type_name, sum(wd.quantity) quantity, mp.status, mp.date_created, mp.date_modify"+
                " FROM((mp_product mp join mp_type_product tp on mp.id_type = tp.id) left join mp_warehouse_detail wd"+
                " on mp.id = wd.id_product) left join(select id from mp_warehouse where status = 'ACTIVE') w"+
                " on w.id = wd.id_warehouse group by wd.id_product, mp.id";

            List<string[]> r = Db.QuerySelect(query, 14);
            if (r == null) { return null; }
            return r.ToArray();
        }

        public string[][] GetByType(string id)
        {
            string query = "select mp.id, mp.name, mp.brand, mp.color, mp.price, mp.img, mp.short_discription, mp.discription," +
                " tp.id type_id, tp.name type_name, sum(wd.quantity) quantity, mp.status, mp.date_created, mp.date_modify" +
                " FROM((mp_product mp join mp_type_product tp on mp.id_type = tp.id) left join mp_warehouse_detail wd" +
                " on mp.id = wd.id_product) left join(select id from mp_warehouse where status = 'ACTIVE') w" +
                " on w.id = wd.id_warehouse where tp.id=@p1 group by wd.id_product, mp.id";
            List<string> param = new List<string>()
            {
                id
            };
            List<string[]> r = Db.QuerySelect(query, 14, param);
            if (r == null) { return null; }
            return r.ToArray();
        }

        public byte[] GetImgProduct(string id)
        {
            string query = "select img from mp_product where id = @p1";
            List<string> param = new List<string>
            {
                id
            };
            return Db.QuerySelectImg(query, param);
        }

        public string[] GetOne(string id)
        {
            string query = "select mp.id, mp.name, mp.brand, mp.color, mp.price, mp.img, mp.short_discription, mp.discription," +
                " tp.id type_id, tp.name type_name, sum(wd.quantity) quantity, mp.status, mp.date_created, mp.date_modify" +
                " FROM((mp_product mp join mp_type_product tp on mp.id_type = tp.id) left join mp_warehouse_detail wd" +
                " on mp.id = wd.id_product) left join(select id from mp_warehouse where status = 'ACTIVE') w" +
                " on w.id = wd.id_warehouse group by wd.id_product, mp.id having mp.id=@p1";
            List<string> param = new List<string>
            {
                id
            };
            List<string[]> r = Db.QuerySelect(query, 14, param);
            if (r == null) { return null; }
            return r.ToArray()[0];
        }

        public bool AddCart(string account, string id_p, string quan)
        {
            string id_cart = Db.QuerySelect("select c.id from mp_cart c join mp_user u on c.id_user = u.id where u.account=@p1",
                1, new List<string>()
                {
                    account
                }).ToArray()[0][0];
            bool ck = Db.Query("insert into mp_cart_detail(id_cart,id_product,quantity) values(@p1,@p2,@p3)",
                new List<string>() 
                { 
                    id_cart,
                    id_p,
                    quan
                }) > 0;
            if (!ck)
            {
                Db.Query("update mp_cart_detail set quantity=quantity+@p1 where id_cart=@p2 and id_product=@p3",
                    new List<string>()
                    {
                        quan,
                        id_cart,
                        id_p
                    });
            }
            return true;
        }
    }
}
