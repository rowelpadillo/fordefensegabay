using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Gabay_Final_V2.Views.Modules.Appointment
{
    public partial class Appointment_Status : System.Web.UI.Page
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["user_ID"] != null)
                {
                    int userID = Convert.ToInt32(Session["user_ID"]);
                    populateAppointmentLabel(userID);
                }
            }

        }
        public void populateAppointmentLabel(int userID)
        {
            // Call the database operation method
            DataTable appointmentData = GetAppointmentData(userID);
            Indication.Text = @"<b>You have successfully booked an appointment.</b><br>
                                Our team is currently verifying the availability of the chosen time and date.<br> 
                                Please stay connected with your email for additional updates regarding your appointment schedule.";

            if (appointmentData.Rows.Count > 0)
            {
                DataRow row = appointmentData.Rows[0];
                appointmentID.Text = row["ID_appointment"].ToString();
                appointmentStatus.Text = row["appointment_status"].ToString();
                DateTime date = (DateTime)row["appointment_date"];
                appointmentDate.Text = date.ToString("MMMM-dd-yyyy");
                appointmentTime.Text = row["appointment_time"].ToString();
                string formattedConcern = row["concern"].ToString().Replace("\n", "<br />");
                appointmentConcern.Text = formattedConcern;
            }
        }
        public DataTable GetAppointmentData(int userID)
        {
            DataTable appointmentData = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT a.ID_appointment, a.appointment_status, a.appointment_date, a.appointment_time, a.concern, a.student_ID
                        FROM appointment a
                        INNER JOIN users_table u ON a.student_ID = u.login_ID
                        WHERE u.user_ID = @userID";

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(appointmentData);
                    }
                }
            }

            return appointmentData;
        }
    }
}