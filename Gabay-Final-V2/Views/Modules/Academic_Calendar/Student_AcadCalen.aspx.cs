using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Gabay_Final_V2.Models.AcadCalen_model;

namespace Gabay_Final_V2.Views.Modules.Academic_Calendar
{
    public partial class Student_AcadCalen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindFilesToDropDownList();
        }

        private void BindFilesToDropDownList()
        {
            AcadCalen_model conn = new AcadCalen_model();
            List<AcadCalen_model.AcademicFile> filesList = conn.FetchFilesDataFromDatabase();
            ddlFiles.Items.Clear(); // Clear existing items

            foreach (AcadCalen_model.AcademicFile file in filesList)
            {
                ListItem item = new ListItem(file.FileName, file.FileId.ToString());
                ddlFiles.Items.Add(item);
            }

            // Set the selected item based on ViewState
            if (ViewState["SelectedFileId"] != null)
            {
                int selectedFileId = (int)ViewState["SelectedFileId"];
                ddlFiles.SelectedValue = selectedFileId.ToString();
            }
        }

        protected void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            AcadCalen_model conn = new AcadCalen_model();
            int selectedFileId = int.Parse(ddlFiles.SelectedValue);
            ViewState["SelectedFileId"] = selectedFileId;

            // Debugging statement to display selectedFileId
            DownloadErrorLabel.Text = "Selected File ID: " + selectedFileId;

            // Fetch the selected file's data and update labels or perform other actions
            byte[] selectedFileData = conn.FetchFileDataFromDatabase(selectedFileId);
            if (selectedFileData != null)
            {
                // Update labels or perform other actions
                DownloadErrorLabel.Text = "";
            }
            else
            {
                DownloadErrorLabel.Text = "Selected file data not found.";
            }
        }

        protected void dwnldLnk_Click(object sender, EventArgs e)
        {
            AcadCalen_model conn = new AcadCalen_model();

            if (ViewState["SelectedFileId"] != null)
            {
                int fileId = (int)ViewState["SelectedFileId"];

                byte[] fileData = conn.FetchFileDataFromDatabase(fileId);
                string fileName = conn.FetchFileNameFromDatabase(fileId);

                if (fileData != null)
                {
                    // Set the response content type to PDF
                    Response.ContentType = "application/pdf";

                    // Set the content disposition to "inline" to open in the browser
                    Response.AddHeader("Content-Disposition", $"inline; filename={fileName}");

                    // Write the file data to the response output stream
                    Response.BinaryWrite(fileData);
                    Response.End();
                }
                else
                {
                    // Handle the case where file data is not available
                    DownloadErrorLabel.Text = "File not found.";
                }
            }
            else
            {
                DownloadErrorLabel.Text = "Please select a file to preview.";
            }
        }
    }
}