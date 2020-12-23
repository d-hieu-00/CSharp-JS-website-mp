using _website_mp.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Models
{
    public class TypeProductModel
    {
        private Database Db { get; set; }

        public TypeProductModel()
        {
            Db = new Database();
        }

        public string[][] GetTypeProductByIdCategory(string id_category)
        {
            string query = "select tp.id, tp.name from mp_type_product tp where tp.id_category = @p1 and tp.status='ACTIVE'";
            int num_col = 2;
            List<string> param = new List<string>
            {
                id_category
            };
            List<string[]> r = Db.QuerySelect(query, num_col, param);
            if (r == null) { return null; }
            return r.ToArray();
        }

        public string[][] GetAll()
        {
            string query = "select tp.id, tp.name, count(p.id_type) quantity, c.id id_category, " +
                " c.name name_category, tp.status, tp.date_created " +
                " from(mp_type_product tp left join mp_product p on tp.id = p.id_type) " +
                " join mp_category c on tp.id_category = c.id " +
                " group by p.id_type, tp.id";
            return Db.QuerySelect(query, 7).ToArray();
        }

        public string[][] GetAllForDisplay()
        {
            string query = "select tp.id, tp.name, count(p.id_type) quantity, c.id id_category, " +
                " c.name name_category, tp.status, tp.date_created " +
                " from(mp_type_product tp left join mp_product p on tp.id = p.id_type) " +
                " join mp_category c on tp.id_category = c.id where tp.status='ACTIVE' " +
                " group by p.id_type, tp.id";
            return Db.QuerySelect(query, 7).ToArray();
        }

        public string[] Get(string id)
        {
            string query = "select * from mp_type_product where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.QuerySelect(query, 6).ToArray()[0];
        }

        public bool ActiveTypeProduct(string id)
        {
            string query = "update mp_type_product set status='ACTIVE' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool DisableTypeProduct(string id)
        {
            string query = "update mp_type_product set status='DISABLE' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool Save(string id, string id_cate, string name)
        {
            string query = "update mp_type_product set id_category=@p1, name=@p2 where id=@p3";
            List<string> param = new List<string>()
            {
                id_cate,
                name,
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool Insert(string name, string id_cate, string status)
        {
            string query = "insert into mp_type_product(name,id_category,status) values(@p1,@p2,@p3)";
            List<string> param = new List<string>()
            {
                name,
                id_cate,
                status
            };
            return Db.Query(query, param) > 0;
        }

        public bool CheckName(string name)
        {
            string query = "select * from mp_type_product where name=@p1)";
            List<string> param = new List<string>()
            {
                name
            };
            return Db.QuerySelect(query, param);
        }
    }
}
