using System.Data;
using System.Data.SqlClient;

namespace DroneSurvey.Models
{
    public class DAL
    {
        public IConfiguration Configuration { get; set; }
        private string? ConnString { get; set; }

        internal DAL()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json");
            Configuration = builder.Build();
            ConnString = Configuration["ConnectionStrings:DefaultConnection"];
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

        internal bool CreateTransaction(string batchUid, string fileName,string filePath)
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

        internal List<ImageUploadHistory> GetImageUploadHistory(string user)
        {
            List<ImageUploadHistory> imageUploads = new();
            ImageUploadHistory imageUpload;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_GetImageUploadHistory"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@User", user);

                        // Executing the SQL query 
                        SqlDataReader sdr = cmd.ExecuteReader();

                        while (sdr.Read())
                        {
                            imageUpload = new();
                            imageUpload.BatchUid = sdr["BatchUid"].ToString();
                            imageUpload.Location = sdr["Location"].ToString();
                            imageUpload.UploadTime = sdr["UploadStartTime"].ToString();
                            imageUpload.UploadStatus = sdr["UploadStatus"].ToString();
                            imageUpload.ContainerUrl = sdr["ContainerUrl"].ToString();
                            imageUpload.ImageUploadCount = Convert.ToInt32(sdr["ImageCount"].ToString());
                            imageUpload.OrthomosaicStatus = sdr["OrthomosaicStatus"].ToString();
                            imageUpload.QCStatus = sdr["QCStatus"].ToString();
                            imageUpload.PolygonStatus = sdr["PolygonStatus"].ToString();
                            imageUpload.PolygonAcceptanceStatus = sdr["IsPolygonAccepted"].ToString();
                            imageUploads.Add(imageUpload);
                        }
                    }

                    conn.Close();
                }
                return imageUploads;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    
        internal ExecutionResponse ValidateLogin(string email, string password)
        {
            ExecutionResponse executionResponse = new();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("dbo.usp_ValidateLogin"))
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        cmd.Parameters.Add("@OutMessage", SqlDbType.VarChar, 200);
                        cmd.Parameters["@OutMessage"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@ExecutionStatus", SqlDbType.Bit);
                        cmd.Parameters["@ExecutionStatus"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        executionResponse.ExecutionStatus = Convert.ToBoolean(cmd.Parameters["@ExecutionStatus"].Value) == true ? ExecutionStatus.Success : ExecutionStatus.Fail;
                        executionResponse.Message = cmd.Parameters["@OutMessage"].Value.ToString();
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                executionResponse.Message =" ";
            }
            
            return executionResponse;
        }
    }
}
