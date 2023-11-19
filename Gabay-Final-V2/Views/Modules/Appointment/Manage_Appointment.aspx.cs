﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
//For Email
using System.IO;
using ZXing;
using ZXing.QrCode;
using MimeKit;
using MailKit.Net.Smtp;
//for json
using System.Configuration;
using System.Web;
using Gabay_Final_V2.Models;
using System.Web.UI.WebControls;
using System.Windows;
using static iTextSharp.text.pdf.PdfDocument;

namespace Gabay_Final_V2.Views.Modules.Appointment
{
    public partial class Manage_Appointment : System.Web.UI.Page
    {
        string connection = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindingAppointment();
            }

            //// para sa notification
            //Manage_Appointment appointmentPage = Page as Manage_Appointment;
            //if (appointmentPage != null)
            //{
            //    appointmentPage.AppointmentStatusChanged += HandleAppointmentStatusChanged;          
            //}
        }

        public class DepartmentUser
        {
            public static event EventHandler AppointmentStatusChanged;

            public static void OnAppointmentStatusChanged(EventArgs e)
            {
                // Raise the event when the appointment status changes.
                AppointmentStatusChanged?.Invoke(null, e);
            }
        }

        private void BindingAppointment()
        {
            if (Session["user_ID"] != null)
            {
                int user_ID = Convert.ToInt32(Session["user_ID"]);
                DataTable dt = fetchAppointBasedOnDepartment(user_ID);

                foreach (DataRow row in dt.Rows)
                {
                    string studentID = (string)row["student_ID"];

                    if (studentID == "guest")
                    {
                        // Set "User Type" to "Guest" for guest appointments
                        row["role"] = "guest";
                    }
                }
               
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        public DataTable fetchAppointBasedOnDepartment(int userID)
        {
            DataTable studentTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                string queryFetchStudent = @"SELECT a.*, ur.role
                                            FROM appointment a
                                            LEFT JOIN users_table u ON a.student_ID = u.login_ID
                                            LEFT JOIN user_role ur ON u.role_ID = ur.role_id
                                            WHERE (a.deptName = (SELECT dept_name FROM department WHERE user_ID = @departmentUserID)
                                            OR a.student_ID = 'guest')
                                            AND (a.student_ID <> 'guest' OR a.deptName = (SELECT dept_name FROM department WHERE user_ID = @departmentUserID))
                                            AND (a.appointment_status = 'pending' OR a.appointment_status = 'reschedule' OR a.appointment_status = 'approved')";
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

        public bool LoadAppointmentModal(int AppointmentID,out string appointmentStats)
        {
            appointmentStats = null;
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
                            appointmentStats = AppointmentStatus.Text;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        protected void ViewConcernModal_Click(object sender, EventArgs e)
        {
            int hiddenID = Convert.ToInt32(HiddenFieldAppointment.Value);

            if (LoadAppointmentModal(hiddenID,out string appointmentStats))
            {
                if( appointmentStats == "approved")
                {
                    approved.Attributes["class"] = "col-8 mb-2 d-grid d-none";
                    reject.Attributes["class"] = "col-4 mb-2 d-grid d-none";
                    resched.Attributes["class"] = "col-12 mb-2 d-none";
                    servedlnk.Attributes["class"] = "col-8 mb-2 d-grid";
                    noShowlnk.Attributes["class"] = "col-4 mb-2 d-grid";
                }
                else
                {
                    approved.Attributes["class"] = "col-8 mb-2 d-grid";
                    reject.Attributes["class"] = "col-4 mb-2 d-grid";
                    resched.Attributes["class"] = "col-12 mb-2";
                    servedlnk.Attributes["class"] = "col-8 mb-2 d-grid d-none";
                    noShowlnk.Attributes["class"] = "col-4 mb-2 d-grid d-none";
                }
            };
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

        public void updateSchedDateTime(int AppointmentID, string newTime, string newDate)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"SELECT * FROM appointment WHERE ID_appointment = @AppointmentID";
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
                            string appointeeEmail = reader["email"].ToString().Trim();
                            string appointee = reader["full_name"].ToString();
                            string destination = reader["deptName"].ToString();
                            string updateStatus = "reschedule";

                            if (newDate != currentDate || newTime != currentTime)
                            {
                                reader.Close();

                                string updateQuery = "UPDATE appointment SET ";
                                if (newDate != currentDate)
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
                                    if (newDate != currentDate)
                                    {
                                        cmdDateTime.Parameters.AddWithValue("@newDate", newDate);
                                    }
                                    if (newTime != currentTime)
                                    {
                                        cmdDateTime.Parameters.AddWithValue("@newTime", newTime);
                                    }
                                    cmdDateTime.Parameters.AddWithValue("@newStatus", updateStatus);
                                    cmdDateTime.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                                    cmdDateTime.ExecuteNonQuery();
                                }

                                // Log the status change in the AppointmentStatusHistory table
                                InsertStatusChangeToHistory(conn, AppointmentID, updateStatus, currentDate, currentTime);

                                // Notify that the status has changed.
                                DepartmentUser.OnAppointmentStatusChanged(EventArgs.Empty);
                            }

                            DateTime newdate = DateTime.Parse(newDate);
                            string formattedNewDate = newdate.ToString("dd MMM, yyyy ddd");

                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("UC Gabay", "noReply@noReply.com"));
                            message.To.Add(new MailboxAddress("Recipient", appointeeEmail));
                            message.Subject = "Appointment Details";

                            var builder = new BodyBuilder();

                            // Add the logo and QR code centered in the email body
                            builder.HtmlBody = $@"
                                <div style='text-align: center;margin-bottom: 10px;'>
                                    <div>
                                        <img src='cid:logo-image' style='width: 100px; height: auto; margin-right: 5px; display: block; margin: 0 auto;'>
                                    </div>
                                    <div style='letter-spacing: 3px; color: #003366; font-weight: 600;'>
                                        GABAY
                                    </div>
                                </div>
                                <div style='text-align: center;'>
                                    <img src='cid:resched-image' width='200' height='200'>
                                </div>";

                            // Add additional appointment details
                            builder.HtmlBody += $@"<div style='text-align: center;'><h1>Heads up!</h1></div>
                                                <div style='text-align: center;'>
                                                <p>Hello!<b> {appointee}</b>, your appointment with an appointment ID: <b>{AppointmentID}</b> scheduled on <b>{currentDate} {currentTime}</b> </p>
                                                <p>has been rescheduled on <b>{formattedNewDate} {newTime}</b>, please go to the GABAY webpage to accept or reject your new appointment status</p>
                                                <p>If you have any concern question kindly visit the {destination}'s office or book another appointment</p>
                                                <p>Thank you!</p>
                                                </div>
                                                <div style='text-align: center;'>
                                                <a href='https://localhost:44341/Landing_Page/LandingPage.aspx' style='display: inline-block; padding: 10px; background-color: #007BFF; color: #fff; text-decoration: none; margin-top: 20px;'>Go to GABAY</a>
                                                </div>";

                            var logoImage = builder.LinkedResources.Add("C:\\Users\\quiro\\source\\repos\\Gabay-Final-V2\\Gabay-Final-V2\\Resources\\Images\\UC-LOGO.png");
                            logoImage.ContentId = "logo-image";
                            logoImage.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                            var qrCodeImage = builder.LinkedResources.Add("C:\\Users\\quiro\\source\\repos\\Gabay-Final-V2\\Gabay-Final-V2\\Resources\\Images\\tempIcons\\reschedule-icon-6.jpg");
                            qrCodeImage.ContentId = "resched-image";
                            qrCodeImage.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                            message.Body = builder.ToMessageBody();

                            // Send the email using MailKit
                            using (var client = new SmtpClient())
                            {
                                client.Connect("smtp.gmail.com", 587, false);
                                client.Authenticate(ConfigurationManager.AppSettings["SystemEmail"], ConfigurationManager.AppSettings["SystemEmailPass"]);
                                client.Send(message);
                                client.Disconnect(true);
                            }
                        }
                    }
                }

                conn.Close();
            }
        }

        private void InsertStatusChangeToHistory(SqlConnection conn, int AppointmentID, string newStatus, string currentDate, string currentTime)
        {
            string insertQuery = "INSERT INTO AppointmentStatusHistory (AppointmentID, StatusChangeDate, PreviousStatus, NewStatus, Notification) " +
                                "VALUES (@AppointmentID, @StatusChangeDate, @PreviousStatus, @NewStatus, @Notification)";

            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                cmd.Parameters.AddWithValue("@StatusChangeDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@PreviousStatus", "current_status");
                cmd.Parameters.AddWithValue("@NewStatus", newStatus);
                cmd.Parameters.AddWithValue("@Notification", "UNREAD");

                cmd.ExecuteNonQuery();
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string newTime = newtime.SelectedValue;
            if (DateTime.TryParse(newdate.Text, out DateTime selectedDate))
            {
                // Format the date as "dd MMM, yyyy"
                string formattedDate = selectedDate.ToString("dd MMM, yyyy");

                // Save the formatted date to a variable or perform any other actions
                string savedDate = formattedDate;

                string successMessage = "Update the schedule to <b>" + HttpUtility.JavaScriptStringEncode(newTime) + "</b> and <b>" + HttpUtility.JavaScriptStringEncode(savedDate) + "</b>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showConfirmationModal",
                    $"$('#confirmationMessage').html('{successMessage}'); $('#ConfirmationModal').modal('show');", true);
            }

        }

        protected void goBacktoReschedModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showReschedModal", "$('#reschedModal').modal('show');", true);
        }

        protected void ApproveLink_Click(object sender, EventArgs e)
        {
            DateTime.TryParse(newdate.Text, out DateTime selectedDate);

            string Appointee = appointmentName.Text;
            string AppointmentID = Label1.Text;
            string appointmentDate = AppointmentDate.Text;
            string appointmentTime = AppointmentTime.Text;

            string ApproveMessage = "<b>Apointment ID:</b> " + HttpUtility.JavaScriptStringEncode(AppointmentID) + "<br />" +
                                       "<b>Appointee: </b>" + HttpUtility.JavaScriptStringEncode(Appointee) + "<br />" +
                                       "<b>Date: </b>" + HttpUtility.JavaScriptStringEncode(appointmentDate) + "<br />" +
                                       "<b>Time: </b>" + HttpUtility.JavaScriptStringEncode(appointmentTime) + "<br /> <hr />" +
                                       "Approve appointment?";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showApproveMessage",
                $"$('#approveMessage').html('{ApproveMessage}'); $('#ApproveModal').modal('show');", true);
        }

        protected void ApproveButton_Click(object sender, EventArgs e)
        {
            try
            {
                int AppointmentID = Convert.ToInt32(HiddenFieldAppointment.Value);
                approveAppointment(AppointmentID);
                BindingAppointment();
                string successMessage = "Schedule is set.";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showSuccessModal",
                    $"$('#successMessage').text('{successMessage}'); $('#successModal').modal('show');", true);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showErrorModal",
                   $"$('#errorMessage').text('{ErrorMessage}'); $('#errorModal').modal('show');", true);
            }
            
        }

        public void approveAppointment(int AppointmentID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string getAppointmentInfoQuery = @"SELECT * FROM appointment WHERE ID_appointment = @AppointmentID";
                conn.Open();
                using (SqlCommand setCmd = new SqlCommand(getAppointmentInfoQuery, conn))
                {
                    setCmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    using (SqlDataReader reader = setCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string appointmentID = reader["ID_appointment"].ToString();
                            string appointee = reader["full_name"].ToString();
                            string destination = reader["deptName"].ToString();
                            DateTime date = (DateTime)reader["appointment_date"];
                            string appointmentDate = date.ToString("dd MMM, yyyy ddd");
                            string appointmentTime = reader["appointment_time"].ToString();
                            string appointeeEmail = reader["email"].ToString();

                            string TempUrl = $"https://localhost:44341/Prototype/tempPage.aspx?appointmentID={appointmentID}";

                            BarcodeWriter barcodeWriter = new BarcodeWriter();
                            barcodeWriter.Format = BarcodeFormat.QR_CODE;
                            barcodeWriter.Options = new QrCodeEncodingOptions
                            {
                                Width = 200,
                                Height = 200,
                            };
                            System.Drawing.Bitmap qrCodeBitmap = barcodeWriter.Write(TempUrl);

                            // Save the QR code as a temporary image file
                            string tempQRCodeFilePath = Server.MapPath("~/TempQRCode.png");
                            qrCodeBitmap.Save(tempQRCodeFilePath, System.Drawing.Imaging.ImageFormat.Png);

                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("UC Gabay", "noReply@noReply.com"));
                            message.To.Add(new MailboxAddress("Recipient", appointeeEmail));
                            message.Subject = "Appointment Details";

                            var builder = new BodyBuilder();

                            // Add the logo and QR code centered in the email body
                            builder.HtmlBody = $@"
                                <div style='text-align: center;margin-bottom: 10px;'>
                                    <div>
                                        <img src='cid:logo-image' style='width: 100px; height: auto; margin-right: 5px; display: block; margin: 0 auto;'>
                                    </div>
                                    <div style='letter-spacing: 3px; color: #003366; font-weight: 600;'>
                                        GABAY
                                    </div>
                                </div>
                                <div style='text-align: center;'>
                                    <img src='cid:qr-code-image' width='200' height='200'>
                                </div>";

                            // Add additional appointment details
                            builder.HtmlBody += $@"<div style='text-align: center;'><h1>Your Appointment is all set!</h1></div>
                                                <div style='text-align: center;'>
                                                <p>Hello!<b> {appointee}</b>, your appointment is set please see the details below</p>
                                                <p><b>Appointment ID:</b> {appointmentID}</p>
                                                <p><b>Schedule:</b> {appointmentDate} {appointmentTime}</p>
                                                <p><b>Destination:</b> {destination}</p>
                                                </div>";

                            var logoImage = builder.LinkedResources.Add("C:\\Users\\quiro\\source\\repos\\Gabay-Final-V2\\Gabay-Final-V2\\Resources\\Images\\UC-LOGO.png");
                            logoImage.ContentId = "logo-image";
                            logoImage.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                            var qrCodeImage = builder.LinkedResources.Add(tempQRCodeFilePath);
                            qrCodeImage.ContentId = "qr-code-image";
                            qrCodeImage.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                            message.Body = builder.ToMessageBody();

                            // Send the email using MailKit
                            using (var client = new SmtpClient())
                            {
                                client.Connect("smtp.gmail.com", 587, false);
                                client.Authenticate(ConfigurationManager.AppSettings["SystemEmail"], ConfigurationManager.AppSettings["SystemEmailPass"]);
                                client.Send(message);
                                client.Disconnect(true);
                            }

                            // Clean up (optional)
                            System.IO.File.Delete(tempQRCodeFilePath);
                        }

                        reader.Close();
                    }
                }


                string query = @"UPDATE appointment SET appointment_status = @AppointmentStats WHERE ID_appointment = @AppointmentID";
                string updateStatus = "approved";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmd.Parameters.AddWithValue("@AppointmentStats", updateStatus);
                    cmd.ExecuteNonQuery();
                }

                // Insert a record into AppointmentStatusHistory
                string queryInsertHistory = @"
                INSERT INTO AppointmentStatusHistory (AppointmentID, StatusChangeDate, PreviousStatus, NewStatus, Notification)
                VALUES (@AppointmentID, @StatusChangeDate, @PreviousStatus, @NewStatus ,@Notification)";
                using (SqlCommand cmdInsertHistory = new SqlCommand(queryInsertHistory, conn))
                {
                    cmdInsertHistory.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmdInsertHistory.Parameters.AddWithValue("@StatusChangeDate", DateTime.Now);
                    cmdInsertHistory.Parameters.AddWithValue("@PreviousStatus", "current_status");
                    cmdInsertHistory.Parameters.AddWithValue("@NewStatus", "approved");
                    cmdInsertHistory.Parameters.AddWithValue("@Notification", "UNREAD");
                    cmdInsertHistory.ExecuteNonQuery();
                }

                // Notify that the status has changed.
                DepartmentUser.OnAppointmentStatusChanged(EventArgs.Empty);

                conn.Close();
            }
        }

        protected void RejectLink_Click(object sender, EventArgs e)
        {
            string rejectMessage = "Reject Appointment?";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "showSuccessModal",
                $"$('#confirmRejectMessage').text('{rejectMessage}'); $('#RejectModal').modal('show');", true);
        }

        protected void rejectAppointmentBtnLink_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showRejectReason", "$('#rejectModal').modal('show');", true);
        }

        protected void rejectBtn_Click(object sender, EventArgs e)
        {
            int AppointmentID = Convert.ToInt32(HiddenFieldAppointment.Value);
            string Rejectreason = rejectReason.Text;
            rejectAppointment(AppointmentID, Rejectreason);
            BindingAppointment();
            string successMessage = "Appointment updated to rejected";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "showSuccessModal",
                $"$('#successMessage').text('{successMessage}'); $('#successModal').modal('show');", true);
        }

        public void rejectAppointment(int AppointmentID, string rejectReason)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string getAppointmentInfoQuery = @"SELECT * FROM appointment WHERE ID_appointment = @AppointmentID";
                conn.Open();
                using (SqlCommand setCmd = new SqlCommand(getAppointmentInfoQuery, conn))
                {
                    setCmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    using (SqlDataReader reader = setCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string appointmentID = reader["ID_appointment"].ToString();
                            string appointee = reader["full_name"].ToString();
                            string destination = reader["deptName"].ToString();
                            DateTime date = (DateTime)reader["appointment_date"];
                            string appointmentDate = date.ToString("dd MMM, yyyy ddd");
                            string appointmentTime = reader["appointment_time"].ToString();
                            string appointeeEmail = reader["email"].ToString();

                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("UC Gabay", "noReply@noReply.com"));
                            message.To.Add(new MailboxAddress("Recipient", appointeeEmail));
                            message.Subject = "Appointment Details";

                            var builder = new BodyBuilder();

                            // Add the logo and QR code centered in the email body
                            builder.HtmlBody = $@"
                                <div style='text-align: center;margin-bottom: 10px;'>
                                    <div>
                                        <img src='cid:logo-image' style='width: 100px; height: auto; margin-right: 5px; display: block; margin: 0 auto;'>
                                    </div>
                                    <div style='letter-spacing: 3px; color: #003366; font-weight: 600;'>
                                        GABAY
                                    </div>
                                </div>
                                <div style='text-align: center;'>
                                    <img src='cid:erro-image' width='200' height='200'>
                                </div>";

                            // Add additional appointment details
                            builder.HtmlBody += $@"<div style='text-align: center;'><h1>Something went wrong!</h1></div>
                                                <div style='text-align: center;'>
                                                <p>Hello!<b> {appointee}</b>, your appointment scheduled on <b>{appointmentDate} {appointmentTime}</b> </p>
                                                <p>that addressed to {destination} is <b>Rejected</b></p>
                                                <p> for the reason of:</p>
                                                <p><b>{rejectReason}</b></p>
                                                <p>If you have any concern question kindly visit the {destination}'s office or book another appointment</p>
                                                <p>Thank you!</p>
                                                </div>";

                            var logoImage = builder.LinkedResources.Add("C:\\Users\\quiro\\source\\repos\\Gabay-Final-V2\\Gabay-Final-V2\\Resources\\Images\\UC-LOGO.png");
                            logoImage.ContentId = "logo-image";
                            logoImage.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                            var qrCodeImage = builder.LinkedResources.Add("C:\\Users\\quiro\\source\\repos\\Gabay-Final-V2\\Gabay-Final-V2\\Resources\\Images\\tempIcons\\error.png");
                            qrCodeImage.ContentId = "erro-image";
                            qrCodeImage.ContentDisposition = new ContentDisposition(ContentDisposition.Inline);

                            message.Body = builder.ToMessageBody();

                            // Send the email using MailKit
                            using (var client = new SmtpClient())
                            {
                                client.Connect("smtp.gmail.com", 587, false);
                                client.Authenticate(ConfigurationManager.AppSettings["SystemEmail"], ConfigurationManager.AppSettings["SystemEmailPass"]);
                                client.Send(message);
                                client.Disconnect(true);
                            }
                        }

                        reader.Close();
                    }
                }
                string query = @"UPDATE appointment SET appointment_status = @AppointmentStats WHERE ID_appointment = @AppointmentID";

                string updateStatus = "rejected";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmd.Parameters.AddWithValue("@AppointmentStats", updateStatus);
                    cmd.ExecuteNonQuery();
                }

                // Insert a record into AppointmentStatusHistory
                string queryInsertHistory = @"
                INSERT INTO AppointmentStatusHistory (AppointmentID, StatusChangeDate, PreviousStatus, NewStatus, Notification)
                VALUES (@AppointmentID, @StatusChangeDate, @PreviousStatus, @NewStatus, @Notification)";
                using (SqlCommand cmdInsertHistory = new SqlCommand(queryInsertHistory, conn))
                {
                    cmdInsertHistory.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmdInsertHistory.Parameters.AddWithValue("@StatusChangeDate", DateTime.Now); // Current date and time
                    cmdInsertHistory.Parameters.AddWithValue("@PreviousStatus", "current_status"); // Replace with the actual previous status
                    cmdInsertHistory.Parameters.AddWithValue("@NewStatus", "rejected"); // New status
                    cmdInsertHistory.Parameters.AddWithValue("@Notification", "UNREAD");
                    cmdInsertHistory.ExecuteNonQuery();
                }

                // Notify that the status has changed.
                DepartmentUser.OnAppointmentStatusChanged(EventArgs.Empty);



                conn.Close();
            }
        }

        protected void served_Click(object sender, EventArgs e)
        {
            try
            {
                int AppointmentID = Convert.ToInt32(HiddenFieldAppointment.Value);
                servedAppointment(AppointmentID);
                BindingAppointment();
                string successMessage = "Appointment completed!";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showSuccessModal",
                    $"$('#successMessage').text('{successMessage}'); $('#successModal').modal('show');", true);
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showErrorModal",
                    $"$('#errorMessage').text('{errorMessage}'); $('#errorModal').modal('show');", true);
            }
            
        }

        public void servedAppointment(int AppointmentID)
        {
            using(SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"UPDATE appointment SET appointment_status = @AppointmentStats WHERE ID_appointment = @AppointmentID";
                conn.Open();
                string updateStatus = "served";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmd.Parameters.AddWithValue("@AppointmentStats", updateStatus);
                    cmd.ExecuteNonQuery();
                }
                string queryInsertHistory = @"
                INSERT INTO AppointmentStatusHistory (AppointmentID, StatusChangeDate, PreviousStatus, NewStatus, Notification)
                VALUES (@AppointmentID, @StatusChangeDate, @PreviousStatus, @NewStatus, @Notification)";
                using (SqlCommand cmdInsertHistory = new SqlCommand(queryInsertHistory, conn))
                {
                    cmdInsertHistory.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmdInsertHistory.Parameters.AddWithValue("@StatusChangeDate", DateTime.Now);
                    cmdInsertHistory.Parameters.AddWithValue("@PreviousStatus", "current_status");
                    cmdInsertHistory.Parameters.AddWithValue("@NewStatus", "served");
                    cmdInsertHistory.Parameters.AddWithValue("@Notification", "UNREAD");

                    cmdInsertHistory.ExecuteNonQuery();
                }


                // Notify that the status has changed.
                DepartmentUser.OnAppointmentStatusChanged(EventArgs.Empty);

                conn.Close();
            }
        }

        protected void noshow_Click(object sender, EventArgs e)
        {
            try
            {
                int AppointmentID = Convert.ToInt32(HiddenFieldAppointment.Value);
                noShowAppointment(AppointmentID);
                BindingAppointment();
                string successMessage = "No Appointee Showed up!";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showSuccessModal",
                    $"$('#successMessage').text('{successMessage}'); $('#successModal').modal('show');", true);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "showErrorModal",
                    $"$('#errorMessage').text('{errorMessage}'); $('#errorModal').modal('show');", true);
            }
        }

        public void noShowAppointment(int AppointmentID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = @"UPDATE appointment SET appointment_status = @AppointmentStats WHERE ID_appointment = @AppointmentID";
                conn.Open();
                string updateStatus = "no show";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmd.Parameters.AddWithValue("@AppointmentStats", updateStatus);
                    cmd.ExecuteNonQuery();
                }
                string queryInsertHistory = @"
                INSERT INTO AppointmentStatusHistory (AppointmentID, StatusChangeDate, PreviousStatus, NewStatus)
                VALUES (@AppointmentID, @StatusChangeDate, @PreviousStatus, @NewStatus)";
                using (SqlCommand cmdInsertHistory = new SqlCommand(queryInsertHistory, conn))
                {
                    cmdInsertHistory.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    cmdInsertHistory.Parameters.AddWithValue("@StatusChangeDate", DateTime.Now);
                    cmdInsertHistory.Parameters.AddWithValue("@PreviousStatus", "current_status");
                    cmdInsertHistory.Parameters.AddWithValue("@NewStatus", "approved");
                    cmdInsertHistory.ExecuteNonQuery();
                }

                // Notify that the status has changed.
                DepartmentUser.OnAppointmentStatusChanged(EventArgs.Empty);

                conn.Close();
            }
        }
    }
}