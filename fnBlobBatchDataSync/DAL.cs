using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fnBlobBatchDataSync
{
    public class DAL
    {
        public IConfiguration Configuration { get; set; }
        private string? ConnString { get; set; }

        internal DAL()
        {
            //var builder = new ConfigurationBuilder()
            //.AddJsonFile("appSettings.json");
            //Configuration = builder.Build();
            //ConnString = Configuration["ConnectionStrings:DefaultConnection"];
            ConnString = "Server=cr-devopsassessment.database.windows.net; Database=DroneSystem; User ID=cr-admin; Password=P455w0rd@12345;";
        }

        internal bool CreateBatch(string batchUid, string dirPath)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_CreateBatch"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@BatchUid", batchUid);
                        cmd.Parameters.AddWithValue("@UploadedBy", "Mansoor R");
                        cmd.Parameters.AddWithValue("@DirPath", dirPath);
                        cmd.Parameters.AddWithValue("@ContainerID", 1);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal bool CreateTransaction(string batchUid, string fileName, string filePath)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_CreateTransaction"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@BatchUid", batchUid);
                        cmd.Parameters.AddWithValue("@ImageName", fileName);
                        cmd.Parameters.AddWithValue("@ImagePath", filePath);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal bool UpdateBatch(string batchUid)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_UpdateBatchStatus"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@BatchUid", batchUid);
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       
    }
}
