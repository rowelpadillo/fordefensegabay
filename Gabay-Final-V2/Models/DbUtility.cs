﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using MailKit.Net.Smtp;
using MimeKit;

namespace Gabay_Final_V2.Models
{
    public class DbUtility
    {
        //mo handle sa mga default na functions:
        //login logic
        //session
        //display students in department
        string connection = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;
        //-------------------------------funcations that applied in the main project-------------------------------//

        //-------------------------------Display all department in a dropdown and get department ID-------------------------------//
        public void ddlDept(DropDownList deptDDL)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = "SELECT ID_dept, dept_name FROM department";

                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListItem item = new ListItem(reader["dept_name"].ToString(), reader["ID_dept"].ToString());
                        deptDDL.Items.Add(item);
                    }
                }
                conn.Close();
            }
        }

        //-------------------------------Login Logic for all users with login credentials-------------------------------//
        public bool loginLogic(string loginID, string loginPass, out int userID, out int roleID, out string loginStatus)
        {
            userID = 0;
            roleID = 0;
            loginStatus = null;

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                string queryLogin = @"SELECT user_ID, role_ID, status FROM users_table WHERE login_ID = @loginID
                                 AND password = @loginPass";

                using (SqlCommand cmd = new SqlCommand(queryLogin, conn))
                {
                    cmd.Parameters.AddWithValue("@loginID", loginID);
                    cmd.Parameters.AddWithValue("@loginPass", loginPass);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userID = Convert.ToInt32(reader["user_ID"]);
                            roleID = Convert.ToInt32(reader["role_ID"]);
                            loginStatus = reader["status"].ToString();
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

        //--------------------------------Fetch Session String for Department-------------------------------//
        public string FetchSessionStringDept(int userID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string queryFetchSession = "SELECT dept_name FROM department WHERE user_ID = @userID";

                using (SqlCommand cmd = new SqlCommand(queryFetchSession, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
        }

        //-------------------------------Fetch Session String for Student-------------------------------//
        public string FetchSessionStringStud(int userID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string queryFetchSession = "SELECT name FROM student WHERE user_ID = @userID";

                using (SqlCommand cmd = new SqlCommand(queryFetchSession, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
        }

        //-------------------------------Fetch Session String for Admin-------------------------------//
        public string FetchSessionStringAdmin(int userID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string queryFetchSession = "SELECT login_ID FROM users_table WHERE user_ID = @userID";

                using (SqlCommand cmd = new SqlCommand(queryFetchSession, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    return cmd.ExecuteScalar()?.ToString();
                }
            }
        }

        //-------------------------------Insert Student in Database-------------------------------//
        public void addStudent(int deptID, string studName, string studAddress, string studCN, string studBOD, string studCY, string studID, string studPass, string studEmail)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string userStatus = "pending";
                string roleType = "student";
                int roleID;

                string query = @"INSERT INTO student (department_ID, name, address, contactNumber, DOB, course_year, studentID, stud_pass, email) " +
                               "VALUES (@deptID, @studName, @studAddress, @studCN, @studBOD, @studCY, @studID, @studPass, @studEmail)";

                string roleQuery = @"SELECT role_id FROM user_role WHERE role = @roleType";

                string userQuery = @"INSERT INTO users_table (role_ID, login_ID, password, status)
                                     VALUES (@role_ID, @login_ID, @password, @userStatus)";

                string updateDeptQuery = @"UPDATE student SET user_ID = (SELECT user_ID FROM users_table WHERE login_ID = @studID)
                                           WHERE studentID = @studID";

                string checkDeptQuery = @"SELECT COUNT(*) FROM student WHERE studentID = @studID";
                string checkUserQuery = @"SELECT COUNT(*) FROM users_table WHERE login_ID = @studID";


                using (SqlCommand roleCmd = new SqlCommand(roleQuery, conn))
                {
                    roleCmd.Parameters.AddWithValue("@roleType", roleType);
                    roleID = Convert.ToInt32(roleCmd.ExecuteScalar());
                }

                using (SqlCommand checkStudCmd = new SqlCommand(checkDeptQuery, conn))
                {
                    checkStudCmd.Parameters.AddWithValue("@studID", studID);
                    int existStud = Convert.ToInt32(checkStudCmd.ExecuteScalar());

                    if (existStud > 0)
                    {
                        return;
                    }
                }

                using (SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn))
                {
                    checkUserCmd.Parameters.AddWithValue("@studID", studID);
                    int existUser = Convert.ToInt32(checkUserCmd.ExecuteScalar());

                    if (existUser > 0)
                    {
                        return;
                    }
                }

                using (SqlCommand insertCmd = new SqlCommand(userQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@role_ID", roleID);
                    insertCmd.Parameters.AddWithValue("@login_ID", studID);
                    insertCmd.Parameters.AddWithValue("@password", studPass);
                    insertCmd.Parameters.AddWithValue("@userStatus", userStatus);

                    insertCmd.ExecuteNonQuery();
                }

                using (SqlCommand studCmd = new SqlCommand(query, conn))
                {
                    studCmd.Parameters.AddWithValue("@deptID", deptID);
                    studCmd.Parameters.AddWithValue("@studName", studName);
                    studCmd.Parameters.AddWithValue("@studAddress", studAddress);
                    studCmd.Parameters.AddWithValue("@studCN", studCN);
                    studCmd.Parameters.AddWithValue("@studBOD", studBOD);
                    studCmd.Parameters.AddWithValue("@studCY", studCY);
                    studCmd.Parameters.AddWithValue("@studID", studID);
                    studCmd.Parameters.AddWithValue("@studPass", studPass);
                    studCmd.Parameters.AddWithValue("@studEmail", studEmail);

                    studCmd.ExecuteNonQuery();
                }

                using (SqlCommand updateCmd = new SqlCommand(updateDeptQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@loginID", studID);
                    updateCmd.Parameters.AddWithValue("@studID", studID);

                    updateCmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }
        public void addDept(string deptName, string deptLogin, string deptPass, string dept_Head, string dept_Desc, string dept_CN, string dept_Email, string dept_Hour)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string roleType = "department";
                string userStatus = "activated";
                int roleID;

                string deptQuery = @"INSERT INTO department (dept_name, departmentID, dept_pass,
                                 dept_head, dept_description, contactNumber, email,office_hour)
                                 VALUES (@deptName, @deptLogin, @deptPass, @deptHead,
                                 @deptDescript, @contactNumber, @deptEmail, @deptHour)";

                string roleQuery = @"SELECT role_id FROM user_role WHERE role = @roleType";

                string userQuery = @"INSERT INTO users_table (role_ID, login_ID, password, status)
                             VALUES (@role_ID, @login_ID, @password, @userStatus)";

                string updateDeptQuery = @"UPDATE department SET user_ID = (SELECT user_ID FROM users_table WHERE login_ID = @loginID)
                                   WHERE departmentID = @deptLogin";

                string checkDeptQuery = @"SELECT COUNT(*) FROM department WHERE departmentID = @deptLogin";
                string checkUserQuery = @"SELECT COUNT(*) FROM users_table WHERE login_ID = @deptLogin";

                using (SqlCommand roleCmd = new SqlCommand(roleQuery, conn))
                {
                    roleCmd.Parameters.AddWithValue("@roleType", roleType);
                    roleID = Convert.ToInt32(roleCmd.ExecuteScalar());
                }

                using (SqlCommand checkDeptCmd = new SqlCommand(checkDeptQuery, conn))
                {
                    checkDeptCmd.Parameters.AddWithValue("@deptLogin", deptLogin);
                    int existDept = Convert.ToInt32(checkDeptCmd.ExecuteScalar());

                    if (existDept > 0)
                    {
                        return;
                    }
                }

                using (SqlCommand checkUserCmd = new SqlCommand(checkUserQuery, conn))
                {
                    checkUserCmd.Parameters.AddWithValue("@deptLogin", deptLogin);
                    int existUser = Convert.ToInt32(checkUserCmd.ExecuteScalar());

                    if (existUser > 0)
                    {
                        return;
                    }
                }

                using (SqlCommand insertCmd = new SqlCommand(userQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@role_ID", roleID);
                    insertCmd.Parameters.AddWithValue("@login_ID", deptLogin);
                    insertCmd.Parameters.AddWithValue("@password", deptPass);
                    insertCmd.Parameters.AddWithValue("@userStatus", userStatus);

                    insertCmd.ExecuteNonQuery();
                }

                using (SqlCommand deptCmd = new SqlCommand(deptQuery, conn))
                {
                    deptCmd.Parameters.AddWithValue("@deptName", deptName);
                    deptCmd.Parameters.AddWithValue("@deptLogin", deptLogin);
                    deptCmd.Parameters.AddWithValue("@deptPass", deptPass);
                    deptCmd.Parameters.AddWithValue("@deptHead", dept_Head);
                    deptCmd.Parameters.AddWithValue("@deptDescript", dept_Desc);
                    deptCmd.Parameters.AddWithValue("@contactNumber", dept_CN);
                    deptCmd.Parameters.AddWithValue("@deptEmail", dept_Email);
                    deptCmd.Parameters.AddWithValue("@deptHour", dept_Hour);

                    deptCmd.ExecuteNonQuery();
                }

                using (SqlCommand updateCmd = new SqlCommand(updateDeptQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@loginID", deptLogin);
                    updateCmd.Parameters.AddWithValue("@deptLogin", deptLogin);

                    updateCmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        //-------------------------------Display/Fetch Students with Pending Status in Department homepage-------------------------------//
        public DataTable displayPendingStudents(int userID)
        {
            DataTable studentTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                string queryFetchStudent = @"SELECT s.name, s.address, s.contactNumber, s.course_year, s.studentID, s.email
                                            FROM student s
                                            INNER JOIN department d ON s.department_ID = d.ID_dept
                                            INNER JOIN users_table u ON s.user_ID = u.user_ID
                                            WHERE d.user_ID = @userID 
                                            AND u.status = 'pending'";
                using (SqlCommand cmd = new SqlCommand(queryFetchStudent, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        studentTable.Load(reader);
                    }
                }
            }
            return studentTable;
        }
        ///-------------------------------Emai and Update process for students-------------------------------//
        public void updateStudentStatus(string studentID)
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();

                string updateQuery = "UPDATE users_table SET status = 'activated' WHERE login_ID = @studentID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@studentID", studentID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Tuple<string, string> getStudEmailInfo(string studentID)
        {
            string studEmail = "";
            string studeName = "";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string queryStudEmail = "SELECT email, name FROM student WHERE studentID = @student_ID";

                using (SqlCommand cmd = new SqlCommand(queryStudEmail, conn))
                {
                    cmd.Parameters.AddWithValue("@student_ID", studentID);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        studEmail = reader["email"].ToString();
                        studeName = reader["name"].ToString();
                    }
                }
            }
            Tuple<string, string> result = new Tuple<string, string>(studEmail, studeName);
            return result;
        }
        public void emailApprovedAccount(string studentEmail, string studentName)
        {
            string emailSubject = "Account Verified";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("UC Gabay", "noreply@gmail.com"));
            message.To.Add(new MailboxAddress(studentName, studentEmail));
            message.Subject = emailSubject;

            var builder = new BodyBuilder();
            builder.HtmlBody = @"<p>Dear "+studentName+"," +
                "Your account has been verified and activated.</p> <p>Follo the link here <a href='https://localhost:44341/Views/LoginPages/Student_login.aspx'>Gabay Login</a> to login your account";

            message.Body = builder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate(ConfigurationManager.AppSettings["SystemEmail"], ConfigurationManager.AppSettings["SystemEmailPass"]);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending error: " + ex.Message);
            }

        }

    }
}