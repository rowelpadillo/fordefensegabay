using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Models
{
    public class AcadCalen_model
    {
        //Database handling sa Academic Calendar need pa ni i fix kay dli mo fetch 
        //pero mo gana ang sa drop down
        //I add pa ang sa admin na side
        private string connStr = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;

        public class AcademicFile
        {
            public int FileId { get; set; }
            public string FileName { get; set; }
            public byte[] FileData { get; set; }
        }

        public List<AcademicFile> FetchFilesDataFromDatabase()
        {
            List<AcademicFile> filesList = new List<AcademicFile>();

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                string query = "SELECT FileId, FileName, FileData FROM UploadedFiles";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AcademicFile file = new AcademicFile
                            {
                                FileId = reader.GetInt32(0),
                                FileName = reader.GetString(1),
                                FileData = (byte[])reader["FileData"]
                            };
                            filesList.Add(file);
                        }
                    }
                }
            }
            return filesList;
        }
        public byte[] FetchFileDataFromDatabase(int fileId)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                string selectQuery = "SELECT FileData FROM UploadedFiles WHERE FileId = @FileId";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@FileId", fileId);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return (byte[])result;
                    }
                }
            }

            return null;
        }
        public string FetchFileNameFromDatabase(int fileId)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();

                string selectQuery = "SELECT FileName FROM UploadedFiles WHERE FileId = @FileId";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@FileId", fileId);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                }
            }

            return null;
        }
    }
}