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

        public string[][] GetAllForDisplay()
        {
            string query = "select mp.id, mp.name, mp.brand, mp.color, mp.price, mp.img, mp.short_discription, mp.discription," +
                " tp.id type_id, tp.name type_name, sum(wd.quantity) quantity, mp.status, mp.date_created, mp.date_modify" +
                " FROM((mp_product mp join mp_type_product tp on mp.id_type = tp.id) left join mp_warehouse_detail wd" +
                " on mp.id = wd.id_product) left join(select id from mp_warehouse where status = 'ACTIVE') w" +
                " on w.id = wd.id_warehouse where tp.status='ACTIVE' and mp.status='ACTIVE' group by wd.id_product, mp.id";

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
                " on w.id = wd.id_warehouse where tp.id=@p1 and tp.status='ACTIVE' and mp.status='ACTIVE' " +
                " group by wd.id_product, mp.id";
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

        public bool ActiveProduct(string id)
        {
            string query = "update mp_product set status='ACTIVE' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool DisableProduct(string id)
        {
            string query = "update mp_product set status='DISABLE' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public string[] Get(string id)
        {
            string query = "select * from mp_product where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.QuerySelect(query, 12, param).ToArray()[0];
        }

        public string[][] GetWarehouse(string id)
        {
            string query = "select wd.id_warehouse, wd.quantity from mp_warehouse_detail wd join mp_warehouse w " +
                " on wd.id_warehouse = w.id where wd.id_product =? and w.status = 'ACTIVE'";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.QuerySelect(query, 2, param).ToArray();
        }

        public bool Save(string name, string brand, string color, string price,
            byte[] img,  string short_discription, string discription, string id_type, string id)
        {
            string query = "update mp_product set img=@p1, name=@p2, brand=@p3, color=@p4, price=@p5, " +
                " short_discription=@p6, discription=@p7, id_type=@p8 where id=@p9";
            List<string> param = new List<string>()
            {
                name,
                brand,
                color,
                price,
                short_discription,
                discription,
                id_type,
                id
            };
            return Db.QueryImg(query, img, param) > 0;
        }

        public string Insert(string name, string brand, string color, string price,
            byte[] img, string short_discription, string discription, string id_type)
        {
            string query = "insert into mp_product(name,brand,color,price,img,short_discription,discription,id_type) " +
                " values(@p2,@p3,@p4,@p5,@p1,@p6,@p7,@p8)";
            List<string> param = new List<string>()
            {
                name,
                brand,
                color,
                price,
                short_discription,
                discription,
                id_type
            };
            if (Db.QueryImg(query, img, param) > 0)
            {
                return Db.QuerySelect("select id from mp_product ORDER by id DESC", 1).ToArray()[0][0];
            }
            return null;
        }
    }
}
