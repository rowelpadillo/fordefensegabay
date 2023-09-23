using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Views.DashBoard.Department_Homepage
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

                int userID = Convert.ToInt32(Session["user_ID"]);

                DbUtility conn = new DbUtility();
                DataTable dt = conn.displayPendingStudents(userID);

                pending_table.DataSource = dt;
                pending_table.DataBind();

            if (Session["UpdateSuccess"] != null && (bool)Session["UpdateSuccess"])
            {
                // Display the success modal
                ScriptManager.RegisterStartupScript(this, this.GetType(), "successModalScript", "$('#successModal').modal('show');", true);
            }
        }

        private void reBindPendingTable()
        {
            DbUtility conn = new DbUtility();

            int userID = Convert.ToInt32(Session["user_ID"]);
            DataTable dt = conn.displayPendingStudents(userID);

            pending_table.DataSource = dt;
            pending_table.DataBind();
        }

        protected void apprvBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DbUtility conn = new DbUtility();

                Button btn = (Button)sender; // Get the button that was clicked
                int rowIndex = Convert.ToInt32(btn.Attributes["data-rowindex"]);

                string studentID = pending_table.DataKeys[rowIndex].Value.ToString();

                var studEmailInfo = conn.getStudEmailInfo(studentID);

                string studentEmail = studEmailInfo.Item1;
                string studentName = studEmailInfo.Item2;

                conn.updateStudentStatus(studentID);
                reBindPendingTable();
               
                Session["UpdateSuccess"] = true;


                if (!string.IsNullOrEmpty(studentEmail))
                {
                    conn.emailApprovedAccount(studentEmail, studentName);
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex);
                Session["UpdateSuccess"] = false;

            }
        }
    }
}