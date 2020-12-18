using _website_mp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Models
{
    public class UserModel
    {
        private Database Db { get; set; }

        public UserModel()
        {
            Db = new Database();
        }

        public bool CheckAccount(string Account)
        {
            string query = "select id from view_user where account=@p1";
            List<string> param = new List<string>
            {
                Account
            };
            return Db.QuerySelect(query, param);
        }

        public bool CheckActive(string Account)
        {
            string query = "select id from view_user where status='ACTIVE' and account=@p1";
            List<string> param = new List<string>
            {
                Account
            };
            return Db.QuerySelect(query, param);
        }

        public bool CheckPassword(string Account, string Password)
        {
            string query = "select id from view_user where account = @p1 and password = @p2";
            List<string> param = new List<string>
            {
                Account,
                Password
            };
            return Db.QuerySelect(query, param);
        }

        public bool CheckPhone(string Phone)
        {
            string query = "select * from view_user where phone = @p1";
            List<string> param = new List<string>
            {
                Phone
            };
            return Db.QuerySelect(query, param);
        }

        public int CreateAccount(List<string> param)
        {
            string query = "CALL createUser(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";
            return Db.Query(query, param);
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

        public bool UpdateLastAccess(string Account)
        {
            string query = "update mp_customer set date_last_access = SYSDATE() where id_user = @p1";
            List<string> param = new List<string>
            {
                GetIdUser(Account)
            };
            return Db.Query(query, param) > 0;
        }

        public string[] GetProfileUser(string Account)
        {
            string query = "select fullname, email, phone, address, city, province from view_user where account=@p1";
            List<string> param = new List<string>
            {
                Account
            };
            return Db.QuerySelect(query, 6, param).ToArray()[0];
        }

        public byte[] GetImg(string Account)
        {
            string query = "select img from mp_user where account = @p1";
            List<string> param = new List<string>
            {
                Account
            };
            return Db.QuerySelectImg(query, param);
        }

        public bool SetImg(byte[] img, string Account)
        {
            string query = "update mp_user set img=@p1 where account=@p2";
            List<string> param = new List<string>
            {
                Account
            };
            return Db.QueryImg(query, img, param) > 0;
        }

        public bool UpdatePassword(string Account, string Password)
        {
            string query = "update mp_user set password=@p1 where account=@p2";
            List<string> param = new List<string>
            {
                Password,
                Account
            };
            return Db.Query(query, param) > 0;
        }

        public bool DeleteAccount(string Account)
        {
            string query = "update mp_customer set status='DISABLE' where id_user=@p1";
            List<string> param = new List<string>
            {
                GetIdUser(Account)
            };
            return Db.Query(query, param) > 0;
        }

        public bool UpdateAccount(string Account, string FullName, string Email, string Address,
            string City, string Province)
        {
            string query = "update mp_customer set full_name=@p1, email=@p2, address=@p3, " +
                "city=@p4, province=@p5 where id_user=@p6";
            List<string> param = new List<string>
            {
                FullName,
                Email,
                Address,
                City,
                Province,
                GetIdUser(Account)
            };
            return Db.Query(query, param) > 0;
        }

        public string[][] GetAll()
        {
            string query = "select * from view_user";
            return Db.QuerySelect(query, 14).ToArray();
        }

        public bool AcitveAccount(string Account)
        {
            string query = "update mp_customer set status='ACTIVE' where id_user=@p1";
            List<string> param = new List<string>
            {
                GetIdUser(Account)
            };
            return Db.Query(query, param) > 0;
        }
    }
}
