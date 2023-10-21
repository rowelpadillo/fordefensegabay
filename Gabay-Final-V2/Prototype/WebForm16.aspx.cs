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

                string queryFetchStudent = @"SELECT a.*, ur.role
                                            FROM appointment a
                                            INNER JOIN users_table u ON a.student_ID = u.login_ID
                                            INNER JOIN user_role ur ON u.role_ID = ur.role_id
                                            WHERE a.deptName = (SELECT dept_name FROM department WHERE user_ID = @departmentUserID)";
                using (SqlCommand cmd = new SqlCommand(queryFetchStudent, conn))
                {
                    cmd.Parameters.AddWithValue("@departmentUserID", userID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        studentTable.Load(reader);
                    }
                }
            }
            return studentTable;
        }

        //KJ LUAB
        public void LoadAppointmentModal(int AppointmentID)
        {
            // Retrieve the User_ID from the session
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"SELECT * FROM appointment WHERE ID_appointment = @AppointmendID";
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmendID", AppointmentID);
                        
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideExampleModal", "$('#exampleModal').modal('hide');", true);
            HiddenFieldAppointment.Value = "";
        }

        protected void appointmentReschedule_Click(object sender, EventArgs e)
        {
            int AppointmentID = Convert.ToInt32(HiddenFieldAppointment.Value);
            getCurrentSchedule(AppointmentID);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showRescheduleModal", "$('#reschedModal').modal('show');", true);
        }
        public void getCurrentSchedule(int AppointmentID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"SELECT appointment_date, appointment_time, appointment_status FROM appointment WHERE ID_appointment = @AppointmentID";
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {   
                            DateTime date = (DateTime)reader["appointment_date"];
                            CurrentAppointmentDate.Text = date.ToString("dd MMM, yyyy ddd");
                            CurrentAppointmentTime.Text = reader["appointment_time"].ToString();
                            CurrentAppointmentStatus.Text = reader["appointment_status"].ToString();
                            newtime.SelectedValue = reader["appointment_time"].ToString();
                            DateTime date1 = (DateTime)reader["appointment_date"];
                            newdate.Text = date1.ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
        }
        protected void gobackToViewAppointment_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideExampleModal", "$('#exampleModal').modal('show');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showRescheduleModal", "$('#reschedModal').modal('hide');", true);
        }

        protected void closeReschedModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideReschedModal", "$('#reschedModal').modal('hide');", true);
            HiddenFieldAppointment.Value = "";
        }

        protected void updtSchedBtn_Click(object sender, EventArgs e)
        {
            int AppointmentID = Convert.ToInt32(HiddenFieldAppointment.Value);
            string newTime = newtime.SelectedValue.ToString();
            string newDate = newdate.Text;
            updateSchedDateTime(AppointmentID, newTime, newDate);
            BindingAppointment();
            string successMessage = "Schedule updated successfully.";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "showSuccessModal",
                $"$('#successMessage').text('{successMessage}'); $('#successModal').modal('show');", true);
        }

        public void updateSchedDateTime(int AppointmentID, string newTime, string newdate)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"SELECT appointment_date, appointment_time, appointment_status
                 FROM appointment WHERE ID_appointment = @AppointmentID";
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DateTime date = (DateTime)reader["appointment_date"];
                            string currentDate = date.ToString("dd MMM, yyyy ddd");
                            string currentTime = reader["appointment_time"].ToString();
                            string updateStatus = "reschedule";

                            if (newdate != currentDate || newTime != currentTime)
                            {
                                reader.Close();

                                string updateQuery = "UPDATE appointment SET ";
                                if (newdate != currentDate)
                                {
                                    updateQuery += "appointment_date = @newDate, ";
                                }
                                if (newTime != currentTime)
                                {
                                    updateQuery += "appointment_time = @newTime, ";
                                }
                                updateQuery += "appointment_status = @newStatus WHERE ID_appointment = @AppointmentID";

                                using (SqlCommand cmdDateTime = new SqlCommand(updateQuery, conn))
                                {
                                    if (newdate != currentDate)
                                    {
                                        cmdDateTime.Parameters.AddWithValue("@newDate", newdate);
                                    }
                                    if (newTime != currentTime)
                                    {
                                        cmdDateTime.Parameters.AddWithValue("@newTime", newTime);
                                    }
                                    cmdDateTime.Parameters.AddWithValue("@newStatus", updateStatus);
                                    cmdDateTime.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                                    cmdDateTime.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}