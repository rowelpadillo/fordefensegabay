using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Views.Modules.Chatbot
{
    public partial class Department_Chatbot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string greetingMessage = "Hi! How can I assist you today? " + "<br />"+
                "<button class='predefined-button' onclick='buttonClick(\"enroll\")'>Enroll</button>" +
                "<button class='predefined-button' onclick='buttonClick(\"otherOption\")'>Other Option</button>";
            AddBotMessage(greetingMessage);
        }

        private void AddBotMessage(string message)
        {
            string botMessageHtml = $"<div class=\"message-container bot-message\">{message}</div>";
            chatContainer.InnerHtml += botMessageHtml;
        }

        private void AddUserMessage(string message)
        {
            string userMessageHtml = $"<div class=\"message-container user-message\">{message}</div>";
            chatContainer.InnerHtml += userMessageHtml;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            string userInput = txtUserInput.Text;
            AddUserMessage(userInput);
            string lowerInput = userInput.ToLower();

            if (userInput != "" || userInput == null)
            {
                // Handle predefined buttons/links
                if (lowerInput == "enroll")
                {
                    // User clicked the "Enroll" button
                    AddBotMessage("To enroll in computer studies, please follow these steps: ...");
                }
                else if (lowerInput == "otheroption")
                {
                    // User clicked the "Other Option" button
                    AddBotMessage("Here's information about the other option: ...");
                }
                // Add more predefined button/link checks as needed

                // If not a predefined button/link, use your chatbot logic
                else
                {
                    string scriptColumn = Chatbot_model.FindMatchingScript(userInput);
                    AddBotMessage(scriptColumn);
                }
                txtUserInput.Text = string.Empty;
            }
        }
    }
}