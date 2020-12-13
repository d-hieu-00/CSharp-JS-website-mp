using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Web;

namespace _website_mp.Classes
{
    public class Database : IDisposable
    {
        /**
         * 
         * 
         * All querys have param must be like templates below
         * List param has same order
         * select ... from ... where ..=@p1 and ..=@p2,...
         * insert .. into .. values(@p1,@p2,..)
         * 
         */
        public MySqlConnection conn { get; set;  }

        public void Connect(string connectionString)
        {
            conn = new MySqlConnection(connectionString);
        }

        public void Connect()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            conn = new MySqlConnection(configuration["ConnectionStrings:Default"]);
        }


        /**
         * 
         * Convert List<string> param to Array<MySqlParameter>
         * to Add param to string query
         * 
         */
        public Array ConvertParams(List<string> param, int start = 1)
        {
            List<MySqlParameter> temp = new List<MySqlParameter>();
            int i = start;
            foreach(string val in param)
            {
                temp.Add(new MySqlParameter("@p"+ Convert.ToString(i), val));
                i++;
            }
            return temp.ToArray();
        }

        /**
         * 
         * 
         * Run query select with param or non param
         * 
         * Return List<string[]> if select some rows or null if execute fail
         * 
         */
        public List<string[]> QuerySelect(string query, int num_col, List<string> param = null)
        {
            try
            {
                Connect();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                if (param != null)
                {
                    cmd.Parameters.AddRange(ConvertParams(param));
                }
                MySqlDataReader rs = cmd.ExecuteReader();

                List<string[]> result = new List<string[]>();
                while (rs.Read())
                {
                    List<string> t = new List<string>();
                    for (int i = 0; i < num_col; i++)
                    {
                        t.Add(rs[i].ToString());
                    }
                    result.Add(t.ToArray());
                }
                conn.Close();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Dispose();
            }
        }

        /**
         * 
         * like Query Select but only take first row of first col
         * return type byte[]
         * 
         */
        public byte[] QuerySelectImg(string query, List<string> param = null)
        {
            try
            {
                Connect();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                if (param != null)
                {
                    cmd.Parameters.AddRange(ConvertParams(param));
                }
                MySqlDataReader rs = cmd.ExecuteReader();

                byte[] result = null;
                if (rs.Read())
                {
                    result = (byte[])rs[0];
                }
                conn.Close();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Dispose();
            }
        }
        /**
         * 
         * 
         * Run query select with param or non param
         * 
         * Return string is first row of first col or empty string
         * 
         */
        public bool QuerySelect(string query, List<string> param = null)
        {
            try
            {
                Connect();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                if (param != null)
                {
                    cmd.Parameters.AddRange(ConvertParams(param));
                }
                bool rs = cmd.ExecuteReader().HasRows;
                conn.Close();
                return rs;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Dispose();
            }
        }

        /**
         * 
         * 
         * Run query dml ddl not select has param or none
         * 
         * return rows effect or -1 if execute fail
         * 
         */
        public int Query(string query, List<string> param = null)
        {
            try
            {
                Connect();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                if (param != null)
                {
                    cmd.Parameters.AddRange(ConvertParams(param));
                }
                int r = cmd.ExecuteNonQuery();
                conn.Close();
                return r;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                Dispose();
            }
        }
        
        /**
         * 
         * run query has insert update type byte[]
         * query type 
         * insert... (img, ...) values(@p1,...)
         * update... set img=@p1,....
         * 
         */
        public int QueryImg(string query, byte[] img, List<string> param = null)
        {
            try
            {
                Connect();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                MySqlParameter param_img = new MySqlParameter("@p1", MySqlDbType.LongBlob, img.Length)
                {
                    Value = img
                };
                cmd.Parameters.Add(param_img);
                if (param != null)
                {
                    cmd.Parameters.AddRange(ConvertParams(param, 2));
                }
                int r = cmd.ExecuteNonQuery();
                conn.Close();
                return r;
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose() => conn.Dispose();
    }
}
