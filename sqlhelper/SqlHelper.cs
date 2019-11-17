using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace NewCaterDal
{
    public static class SqlHelper
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["sql"].ToString();
        //public static string connStr = @"data source=C:\Users\Administrator\Desktop\ItcastCater.db;version=3;";
        //只执行方法 insert update delete
        //执行命令并返回受影响的函数
        public static int ExecuteNonQuery(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddRange(ps);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        //获取首行首列值得查询
        public static object ExecuteScalar(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(ps);
                conn.Open();
                //执行命令 获取查询结果中的首行首列
                return cmd.ExecuteScalar();
            }
        }
        //获取结果集
        public static DataTable GetDataTable(string sql, params SqlParameter[] ps)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                DataTable dt = new DataTable();
                adapter.SelectCommand.Parameters.AddRange(ps);
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
