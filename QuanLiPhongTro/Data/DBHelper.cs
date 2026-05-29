using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QuanLiPhongTro.Data
{
    public class DBHelper
    {
        public readonly string StudentID = "2415053122232";

        private readonly string _connectionString;

        public DBHelper(IConfiguration configuration)
        {
            Console.WriteLine("2415053122232");
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public int ExecuteNonQuery(string query)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            return rowsAffected;
        }

        public object ExecuteScalar(string query)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }
    }
}