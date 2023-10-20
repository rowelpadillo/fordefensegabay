using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Gabay_Final_V2.Models;

namespace Gabay_Final_V2.Prototype
{
    public partial class WebForm16 : System.Web.UI.Page
    {
        string connection = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

            BindingAppointment();
        }

        private void BindingAppointment()
        {
            int departmentUserID = 4053;
            DataTable dt = fetchAppointBasedOnDepartment(departmentUserID);

            GridView1.DataSource = dt;
            GridView1.DataBind();
      
        }

        public DataTable fetchAppointBasedOnDepartment(int userID)
        {
            DataTable studentTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string appointmentStatus = "pending";
                string queryFetchStudent = @"SELECT a.*, ur.role
                                            FROM appointment a
                                            INNER JOIN users_table u ON a.student_ID = u.login_ID
                                            INNER JOIN user_role ur ON u.role_ID = ur.role_id
                                            WHERE a.deptName = (SELECT dept_name FROM department WHERE user_ID = @departmentUserID)
                                            AND a.appointment_status = @appointmentStatus";
                using (SqlCommand cmd = new SqlCommand(queryFetchStudent, conn))
                {
                    cmd.Parameters.AddWithValue("@departmentUserID", userID);
                    cmd.Parameters.AddWithValue("@appointmentStatus", appointmentStatus);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        studentTable.Load(reader);
                    }
                }
            }
            return studentTable;
        }

        //KJ LUAB
        //public void LoadAppointmentModal(int AppointmendID)
        //{
        //    // Retrieve the User_ID from the session
        //    if (Session["user_ID"] != null)
        //    {
        //        int user_ID = Convert.ToInt32(Session["user_ID"]);

        //        using (SqlConnection conn = new SqlConnection(connection))
        //        {
        //            string query = @"SELECT * FROM appointment WHERE ID_appointment = @AppointmendID AND User_ID = @user_ID";
        //            conn.Open();
        //            using (SqlCommand cmd = new SqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@ID_appointment", AppointmendID);
        //                cmd.Parameters.AddWithValue("@user_ID", user_ID);
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        Titlebx.Text = reader["Title"].ToString();
        //                        DateTime date = (DateTime)reader["Date"];
        //                        Datebx.Text = date.ToString("yyyy-MM-dd");
        //                        ShortDescbx.Text = reader["ShortDescription"].ToString();
        //                        DtlDescBx.Text = reader["DetailedDescription"].ToString();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // Handle the case when user_ID is not available in the session
        //        // You can throw an exception, show an error message, or take appropriate action.
        //        throw new Exception("User_ID not available in the session.");
        //    }
        //}

        //protected void gridviewAppointment_Click(object sender, EventArgs e)
        //{
        //    int hiddenID = Convert.ToInt32(HiddenFieldAppointment.Value);
        //    LoadAppointmentModal(hiddenID);
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "showEditModal", "$('#toEditModal').modal('show');", true);
        //}


    }
}