using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Prototype
{
    public partial class WebForm10 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string greetingMessage = "Hi! How can I assist you today? " + "<br />" +
                "<div class='button-container text-center'>" +
                "<button class='btn predefined-button' onclick='buttonClick(\"enrollment\")'>Enrollment</button>" +
                "<button class='btn predefined-button' onclick='buttonClick(\"tuition payment\")'>Tuition Payment</button>" +
                "</div>";
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
                if (lowerInput == "enrollment")
                {
                    // User clicked the "Enroll" button
                    AddBotMessage("To enroll in computer studies, please follow these steps: ...");
                }
                else if (lowerInput == "tuition payment")
                {
                    // User clicked the "Other Option" button
                    AddBotMessage("To pay tuition fee just approach the guard to get payment form and after that fill up the form then you can proceed in the cashier.");
                }
                // Add more predefined button/link checks as needed

                // If not a predefined button/link, use your chatbot logic
                else if (lowerInput == "hi")
                {
                    AddBotMessage("Hello! what can I assist to you today?");
                }
                else if (lowerInput == "where")
                {
                    AddBotMessage("");
                }
                else
                {
                    string scriptColumn = Chatbot_model.FindMatchingScript(userInput);
                    AddBotMessage(scriptColumn);
                }
                txtUserInput.Text = string.Empty;
            }
        }
        private static string conn = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;

        public DataTable dt()
        {
            DataTable datable = new DataTable();

            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = "SELECT * FROM response";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                connection.Open();
                adapter.Fill(datable);
            }
            return datable;
        }

        public void AddResponse(string scripts, string keywords)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = "INSERT INTO response (response, keywords) VALUES (@response, @keywords)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@response", scripts);
                command.Parameters.AddWithValue("@keywords", keywords);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void UpdateResponse(int scriptID, string updatedScript, string updatedKeywords)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = "UPDATE response SET response = @Script, keywords = @Keywords WHERE res_ID = @ScriptId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Script", updatedScript);
                command.Parameters.AddWithValue("@Keywords", updatedKeywords);
                command.Parameters.AddWithValue("@ScriptId", scriptID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void DeleteResponse(int scriptID)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = "DELETE FROM response WHERE res_ID = @ScriptId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScriptId", scriptID);

                connection.Open();
                command.ExecuteNonQuery();

            }
        }
        public static string FindMatchingScript(string userInput)
        {
            Dictionary<string, int> keywordCount = new Dictionary<string, int>();

            // Tokenize the user input
            string[] userTokens = userInput.ToLower().Split(' ');

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();

                string selectQuery = "SELECT response, keywords FROM Chat_Response";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string script = reader.GetString(0);
                            string keywords = reader.GetString(1);
                            int count = 0;

                            foreach (string keyword in keywords.Split(','))
                            {
                                if (userTokens.Contains(keyword.Trim(), StringComparer.OrdinalIgnoreCase))
                                {
                                    count++;
                                }
                            }

                            if (!keywordCount.ContainsKey(script))
                            {
                                keywordCount.Add(script, count);
                            }
                            else
                            {
                                keywordCount[script] += count;
                            }
                        }
                    }
                }
            }

            // Find the script with the highest keyword count
            string bestScript = "";
            int maxCount = 0;

            foreach (var pair in keywordCount)
            {
                if (pair.Value > maxCount)
                {
                    maxCount = pair.Value;
                    bestScript = pair.Key;
                }
            }

            if (maxCount == 0)
            {
                bestScript = "I'm sorry, I didn't understand your question. Could you please rephrase it?";
            }

            return bestScript;
        }

    }
}