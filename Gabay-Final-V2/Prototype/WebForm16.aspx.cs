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
        int departmentUserID = 4053;
        protected void Page_Load(object sender, EventArgs e)
        {

            BindingAppointment();
            
        }

        private void BindingAppointment()
        {
           
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
        public void LoadAppointmentModal(int AppointmendID)
        {
            // Retrieve the User_ID from the session
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"SELECT * FROM appointment WHERE ID_appointment = @AppointmendID";
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmendID", AppointmendID);
                        
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                         if (reader.Read())
                         {
                             appointmentName.Text = reader["full_name"].ToString();
                             Label1.Text = reader["ID_appointment"].ToString();
                             DateTime date = (DateTime)reader["appointment_date"];
                             AppointmentDate.Text = date.ToString("dd MMM, yyyy ddd");
                             AppointmentTime.Text = reader["appointment_time"].ToString();
                             appointmentConcern.Text = reader["concern"].ToString();
                            AppointmentStatus.Text = reader["appointment_status"].ToString();
                         }
                    }
                }
            }
        }

        protected void ViewConcernModal_Click(object sender, EventArgs e)
        {
            int hiddenID = Convert.ToInt32(HiddenFieldAppointment.Value);

            LoadAppointmentModal(hiddenID);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showExampleModal", "$('#exampleModal').modal('show');", true);
        }
        

        protected void CloseViewModal_Click(object sender, EventArgs e)
        {
            CloseView();
        }

        public void CloseView()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideExampleModal", "$('#exampleModal').modal('hide');", true);
            HiddenFieldAppointment.Value = "";
        }

        protected void appointmentReschedule_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showRescheduleModal", "$('#reschedModal').modal('show');", true);
        }
    }
}