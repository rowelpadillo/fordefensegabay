using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Views.DashBoard.Student_Homepage
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private string connStr = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAnnouncements();
            }
        }
        public DataTable GetAnnouncements()
        {
            if (Session["user_ID"] != null)
            {
                int user_ID = Convert.ToInt32(Session["user_ID"]);
                string query = @"SELECT A.*
                     FROM Announcement A
                     INNER JOIN department D ON A.User_ID = D.user_ID
                     INNER JOIN student S ON D.ID_dept = S.department_ID
                     WHERE S.user_ID = @user_ID";

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@user_ID", user_ID);

                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);

                    return dt;
                }
            }
            else
            {
                return null;
            }
               
        }
        protected void LoadAnnouncements()
        {

            DataTable dt = GetAnnouncements();

            rptAnnouncements.DataSource = dt;
            rptAnnouncements.DataBind();
        }
    }
}