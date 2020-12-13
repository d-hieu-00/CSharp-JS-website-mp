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
        private Database db { get; set; }

        public TypeProductModel()
        {
            db = new Database();
        }

        public string[][] getTypeProductByIdCategory(string id_category)
        {
            string query = "select tp.id, tp.name from mp_type_product tp where tp.id_category = @p1 ";
            int num_col = 2;
            List<string> param = new List<string>
            {
                id_category
            };
            List<string[]> r = db.QuerySelect(query, num_col, param);
            if (r == null) { return null; }
            return r.ToArray();
        }
    }
}
