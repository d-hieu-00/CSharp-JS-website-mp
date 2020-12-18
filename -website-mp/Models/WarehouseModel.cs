using _website_mp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Models
{
    public class WarehouseModel
    {
        private Database Db { get; set; }

        public WarehouseModel()
        {
            Db = new Database();
        }

        public string[][] GetAll()
        {
            string query = "select * from mp_warehouse";
            return Db.QuerySelect(query, 8).ToArray();
        }

        public bool ActiveWarehouse(string id)
        {
            string query = "update mp_warehouse set status='ACTIVE' where id=@p1";
            List<string> param = new List<string>
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool DisableWarehouse(string id)
        {
            string query = "update mp_warehouse set status='DISABLE' where id=@p1";
            List<string> param = new List<string>
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool Save(string id, string name, string address, string city, string province)
        {
            string query = "update mp_warehouse set name=@p1, city=@p2, province=@p3, address=@p4 where id=@p5";
            List<string> param = new List<string>
            {
                name,
                city,
                province,
                address,
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool Insert(string name, string city, string province, string address, string status)
        {
            string query = "insert into mp_warehouse(name,city,province,address,status) " + 
                "values(@p1,@p2,@p3,@p4,@p5)";
            List<string> param = new List<string>
            {
                name,
                city,
                province,
                address,
                status
            };
            return Db.Query(query, param) > 0;
        }

        public bool CheckName(string name)
        {
            string query = "select * from mp_warehouse where name=@p1";
            List<string> param = new List<string>
            {
                name
            };
            return Db.QuerySelect(query, param);
        }

        public string[][] Details(string id)
        {
            string query = "select p.name, p.brand, wd.quantity, wd.date_created, wd.date_modify " +
                    " FROM((select * from mp_product where status = 'ACTIVE') p " +
                    " join mp_warehouse_detail wd on p.id = wd.id_product) where wd.id_warehouse =@p1 ";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.QuerySelect(query, 5, param).ToArray();
        }

        public string[][] DetailProduct(string id_p)
        {
            string query = "select w.id, w.name, wd.quantity, concat(w.name,' - ',wd.quantity) display " +
                    " from mp_warehouse_detail wd join mp_warehouse w on wd.id_warehouse = w.id where wd.id_product =@p1 ";
            List<string> param = new List<string>()
            {
                id_p
            };
            return Db.QuerySelect(query, 4, param).ToArray();
        }

        public bool UpdateWarehouseDetail(string quan, string id_p, string id_w)
        {
            string query = "update mp_warehouse_detail set quantity=@p1 where id_product=@p2 and id_warehouse=@p3";
            List<string> param = new List<string>()
            {
                quan,
                id_p,
                id_w
            };
            return Db.Query(query, param) > 0;
        }

        public bool InsertWarehouseDetail(string quan, string id_p, string id_w)
        {
            string query = "insert into mp_warehouse_detail(quantity,id_product,id_warehouse) values(@p1,@p2,@p3)";
            List<string> param = new List<string>()
            {
                quan,
                id_p,
                id_w
            };
            return Db.Query(query, param) > 0;
        }

        public bool DeleteWarehouseDetail(string id_p, string id_w)
        {
            string query = "delete from mp_warehouse_detail where id_product=@p1 and id_warehouse=@p2";
            List<string> param = new List<string>()
            {
                id_p,
                id_w
            };
            return Db.Query(query, param) > 0;
        }

        public bool RestoreWarehouse(string quan, string id_p, string id_w)
        {
            string query = "update mp_warehouse_detail set quantity=quantity+@p1 where id_product=@p2 and id_warehouse=@p3";
            List<string> param = new List<string>()
            {
                quan,
                id_p,
                id_w
            };
            return Db.Query(query, param) > 0;
        }

        public bool DeleteAllWarehouseDetails(string id_p)
        {
            string query = "delete from mp_warehouse_detail where id_product=@p1";
            List<string> param = new List<string>()
            {
                id_p
            };
            return Db.Query(query, param) > 0;
        }
    }
}
