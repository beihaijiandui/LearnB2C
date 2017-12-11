using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility
{
    /// <summary>
    /// 实现所有对SQL Server数据库的所有访问操作
    /// </summary>
    public class SqlDBHelp
    {
        private static string _connStr = "server=.uid=sa;pwd=;database=B2C";
        private static SqlConnection sqlcon;

        /// <summary>
        /// 获取一个可用于数据库操作的连接类
        /// 注意：这是一个属性不是函数，因为没有()和参数
        /// </summary>
        private static SqlConnection Connection
        {
            get
            {
                if (sqlcon == null)
                {
                    sqlcon = new SqlConnection(_connStr);
                    sqlcon.Open();
                }
                else if (sqlcon.State == ConnectionState.Broken || sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Close();
                    sqlcon.Open();
                }
                return sqlcon;
            }
        }
        /// <summary>
        /// 根据查询的语句返回执行受影响的行数
        /// </summary>
        /// <param name="strsql">Insert、Update、Delete语句</param>
        /// <returns></returns>
        public static int GetExecute(string strsql)
        {
            try
            {
                SqlCommand sqlcmd = new SqlCommand(strsql, Connection);
                int i = sqlcmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private static void CloseConnection()
        {
            if (sqlcon != null)
            {
                sqlcon.Close();
            }
        }
        /// <summary>
        /// 根据查询的语句获取查询的结果集
        /// </summary>
        /// <param name="strsql">Select语句</param>
        /// <returns>查询的结果-表数据</returns>
        public static DataTable GetTable(string strsql)
        {
            DataTable dt = null;
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(strsql, Connection);
                dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
