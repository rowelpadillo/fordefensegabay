using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Views.LoginPages
{
    public partial class Admin_login : System.Web.UI.Page
    {
        protected void btn_deptLogin_Click(object sender, EventArgs e)
        {
            string loginID = login_ID.Text;
            string loginPass = login_Pass.Text;

            DbUtility conn = new DbUtility();

            if (conn.loginLogic(loginID, loginPass, out int userID, out int roleID, out string loginStatus))
            {
                Session["user_ID"] = userID;
                Session["role_ID"] = roleID;

                if (roleID != 1)
                {
                    errorDiv.Attributes["class"] = "alert alert-danger";
                    pendingError.Attributes["class"] = "alert alert-danger d-none";
                }
                else
                {
                    if (loginStatus == "pending")
                    {
                        pendingError.Attributes["class"] = "alert alert-danger";
                        errorDiv.Attributes["class"] = "alert alert-danger d-none";
                    }
                    else
                    {
                        Response.Redirect("..\\DashBoard\\Admin_Homepage\\Admin_DashBoard.aspx");
                    }
                }
            }
            else
            {
                errorDiv.Attributes["class"] = "alert alert-danger";
            }
        }
    }
}