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

        public string GetIdUser(string Account)
        {
            string query = "select id from view_user where account=@p1";
            List<string> param = new List<string>
            {
                Account
            };
            return Db.QuerySelect(query, 1, param).ToArray()[0][0];
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
        /*public function order($FullName, $Phone, $Address, $City, $Province, $Total, $Cd, $Account){
                if($this->db->rowCount() > 0){
                    $this->db->Query('select id from mp_order order by id DESC');
                    $id_order = $this->db->fetch();
                    for($i=0; $i<Count($Cd); $i++){
                        $this->db->Query('insert into mp_order_detail(id_order,id_product,quantity,price) values(?,?,?,?)',
                            array($id_order->id,$Cd[$i]['id'],$Cd[$i]['quan'],$Cd[$i]['price']));
                        if($this->db->rowCount() <= 0){
                            $this->db->rollback();
                            return false;
                        }
                    }
                    $this->db->commit();
                    return true;
                } else {
                    $this->db->rollback();
                    return false;
                }
                $this->db->commit();
            }*/
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
