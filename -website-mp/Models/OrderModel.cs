using _website_mp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Models
{
    public class OrderModel
    {
        private Database Db { get; set; }
        public OrderModel()
        {
            Db = new Database();
        }

        public string[][] GetAllOrder()
        {
            string query = "select o.id, o.full_name, o.phone, o.shipping_fee, o.total_price, o.status, " +
                " CONCAT(o.address, ', ', o.province, ', ', o.city) address, u.account, o.date_created, o.date_modify " +
                " FROM mp_order o left join mp_user u on o.id_user = u.id where status != 'Hoàn thành' and status != 'Đã hủy'";
            return Db.QuerySelect(query, 10).ToArray();
        }

        public string[][] GetAllInvoice()
        {
            string query = "select o.id, o.full_name, o.phone, o.shipping_fee, o.total_price, o.status, " +
                " CONCAT(o.address, ', ', o.province, ', ', o.city) address, u.account, o.date_created, o.date_modify " +
                " FROM mp_order o left join mp_user u on o.id_user = u.id where status = 'Hoàn thành'";
            return Db.QuerySelect(query, 10).ToArray();
        }

        public string[][] GetAllOrderCancel()
        {
            string query = "select o.id, o.full_name, o.phone, o.shipping_fee, o.total_price, o.status, " +
                " CONCAT(o.address, ', ', o.province, ', ', o.city) address, u.account, o.date_created, o.date_modify " +
                " FROM mp_order o left join mp_user u on o.id_user = u.id where status = 'Đã hủy'";
            return Db.QuerySelect(query, 10).ToArray();
        }

        public string[] GetDetailOrder(string id_o)
        {
            string query = "select o.id, o.full_name, o.phone, o.shipping_fee, o.address, o.province, o.city " +
                " FROM mp_order o where id=@p1 ";
            List<string> param = new List<string>()
            {
                id_o
            };
            return Db.QuerySelect(query, 7, param).ToArray()[0];
        }
        
        public string[][] GetDetailProductOrder(string id_o)
        {
            string query = "select od.id_product, p.name, od.quantity, od.id_warehouse, od.price from mp_order_detail " +
                " od join mp_product p on od.id_product = p.id where od.id_order=@p1";
            List<string> param = new List<string>()
            {
                id_o
            };
            return Db.QuerySelect(query, 5, param).ToArray();
        }
        
        public bool UpdateOrder(string shipping_fee, string total_price, string full_name,
            string address, string city, string province, string id)
        {
            string query = "update mp_order set shipping_fee=@p1, total_price=@p2, full_name=@p3, " +
                " address =@p4, city =@p5, province =@p6 where id =@p7";
            List<string> param = new List<string>()
            {
                shipping_fee,
                total_price,
                full_name,
                address,
                city,
                province,
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool UpdateDetailOrder(string id_w, string quan, string price, string id_o, string id_p)
        {
            string query = "update mp_order_detail set id_warehouse=@p1, quantity=@p2, price=@p3 " +
                " where id_order =@p4 and id_product =@p5 ";
            List<string> param = new List<string>()
            {
                id_w,
                quan,
                price,
                id_o,
                id_p
            };
            return Db.Query(query, param) > 0;
        }

        public bool UpdateStatusCofirmed(string id)
        {
            string query = "update mp_order set status='Đã xác nhận' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool UpdateStatusDeliver(string id)
        {
            string query = "update mp_order set status='Đang giao' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool UpdateStatusCancel(string id)
        {
            string query = "update mp_order set status='Đã hủy' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public bool UpdateStatusDone(string id)
        {
            string query = "update mp_order set status='Hoàn thành' where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.Query(query, param) > 0;
        }

        public string GetStatus(string id)
        {
            string query = "select status from mp_order where id=@p1";
            List<string> param = new List<string>()
            {
                id
            };
            return Db.QuerySelect(query,1,param).ToArray()[0][0];
        }

        public string EarnMonth()
        {
            string query = "select if(sum(total_price)=null,0,sum(total_price)) r "+
                " from mp_order where status = 'Hoàn thành' and month(date_modify) = month(sysdate())";
            return Db.QuerySelect(query, 1).ToArray()[0][0];
        }

        public string EarnYear()
        {
            string query = "select if(sum(total_price)=null,0,sum(total_price)) r " +
                "from mp_order where status = 'Hoàn thành' and year(date_modify) = year(sysdate())";
            return Db.QuerySelect(query, 1).ToArray()[0][0];
        }

        public string Pending()
        {
            string query = "select count(id) r from mp_order where status='Chờ xác nhận'";
            return Db.QuerySelect(query, 1).ToArray()[0][0];
        }

        public string Shipping()
        {
            string query = "select count(id) r from mp_order where status='Đang giao'";
            return Db.QuerySelect(query, 1).ToArray()[0][0];
        }

        public bool Order(string FullName, string Phone, string Address, string City, string Province, 
            string Total, string Id_user, string[][] Cd)
        {
            bool ck;
            if (Id_user != null)
            {
                string query = "insert into mp_order(id_user,shipping_fee,total_price,full_name,phone,address,city,province) "+
                        " values(@p1, '20000',@p2,@p3,@p4,@p5,@p6,@p7)";
                List<string> param = new List<string>
                {
                    Id_user,
                    Total,
                    FullName,
                    Phone,
                    Address,
                    City,
                    Province
                };
                ck = Db.Query(query, param) > 0;
            }
            else
            {
                string query = "insert into mp_order(shipping_fee,total_price,full_name,phone,address,city,province) " +
                        " values('20000',@p1,@p2,@p3,@p4,@p5,@p6)";
                List<string> param = new List<string>
                {
                    Total,
                    FullName,
                    Phone,
                    Address,
                    City,
                    Province
                };
                ck = Db.Query(query, param) > 0;
            }

            if (ck)
            {
                string query = "select id from mp_order order by id DESC";
                string id_o =  Db.QuerySelect(query, 1).ToArray()[0][0];
                foreach(string[] val in Cd)
                {
                    query = "insert into mp_order_detail(id_order,id_product,quantity,price) values(@p1,@p2,@p3,@p4)";
                    List<string> param = new List<string>
                    {
                        id_o,
                        val[0],
                        val[1],
                        val[2]
                    };
                    if(!(Db.Query(query,param) > 0))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
