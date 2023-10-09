using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Views.Modules.Admin_Modules
{
    public partial class Manage_AcadCalen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindDt();
        }
        public void BindDt()
        {
            AcadCalen_model conn = new AcadCalen_model();
            upldTbl.DataSource = conn.dt();
            upldTbl.DataBind();
        }
        protected void upldBtn_Click(object sender, EventArgs e)
        {
            string Filename = fileName.Text;
            byte[] FileData = fileUpload.FileBytes;

            AcadCalen_model conn = new AcadCalen_model();
            try
            {
                if (fileUpload.HasFile)
                {
                    conn.uploadFile(Filename, FileData);
                    string message = "File uploaded successfully.";
                    string script = $@"<script>document.querySelector('.modal-body').innerHTML = '{message}';</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script);

                    string openModalScript = @"<script>$('#myModal').modal('show');</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "openModal", openModalScript);
                    clearInputs();
                    BindDt();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                string script = $@"<script>document.querySelector('.modal-body').innerHTML = '{message}';</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script);

                string openModalScript = @"<script>$('#myModal').modal('show');</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "openModal", openModalScript);
                clearInputs();
            }
            
        }

        private void clearInputs()
        {
            fileName.Text = string.Empty;
        }

        protected void upldTbl_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;


        }

        protected void upldTbl_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int fileID = Convert.ToInt32(upldTbl.DataKeys[e.RowIndex].Value);

            AcadCalen_model conn = new AcadCalen_model();
            conn.deleteUpldFile(fileID);

            BindDt();
        }
    }
}