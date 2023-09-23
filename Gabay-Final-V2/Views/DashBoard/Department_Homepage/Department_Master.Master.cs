using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Views.DashBoard.Department_Homepage
{
    public partial class Department_Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //This is to disable caching for the dashboard to prevent backward login
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
                //Database connection
                DbUtility conn = new DbUtility();
                //Setting the session ID for the user
                if (Session["user_ID"] != null)
                {
                    int userID = Convert.ToInt32(Session["user_ID"]);

                    string userName = conn.FetchSessionStringDept(userID);

                    lblDept_name.Text = userName;
                }
                else
                {
                    //Redirects user if login credentials is not valid
                    Response.Redirect("..\\..\\..\\Views\\Loginpages\\Department_login.aspx");

                }
            }
        }

        protected void prflBtn_Click(object sender, EventArgs e)
        {
            
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("..\\..\\..\\Views\\Loginpages\\Department_login.aspx");
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("..\\..\\..\\Views\\Loginpages\\Department_login.aspx");
        }
    }
}