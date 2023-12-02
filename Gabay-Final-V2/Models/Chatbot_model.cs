﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using NTextCat;

namespace Gabay_Final_V2.Models
{
    public class Chatbot_model
    {
        private static string conn = ConfigurationManager.ConnectionStrings["Gabaydb"].ConnectionString;

        public DataTable dt()
        {
            DataTable datable = new DataTable();

            using (SqlConnection connection = new SqlConnection(conn))
            {
                string query = "SELECT * FROM Chat_Response";

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
                string query = "INSERT INTO Chat_Response (response, keywords) VALUES (@response, @keywords)";

                scripts = scripts.Replace("<br>", "\n");

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
                string query = "UPDATE Chat_Response SET response = @Script, keywords = @Keywords WHERE res_ID = @ScriptId";

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
                string query = "DELETE FROM Chat_Response WHERE res_ID = @ScriptId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ScriptId", scriptID);

                connection.Open();
                command.ExecuteNonQuery();

            }
        }

        private readonly RankedLanguageIdentifierFactory languageIdentifierFactory;

        public Chatbot_model()
        {
            languageIdentifierFactory = new RankedLanguageIdentifierFactory();
        }

        private int countUnAnswered;

        public int CountUnAnswered
        {
            get { return countUnAnswered; }
            set { countUnAnswered = value; }
        }
        public string FindMatchingScript(string userInput, ref int countUnAnswered)
        {
            Dictionary<string, int> keywordCount = new Dictionary<string, int>();

            // Tokenize the user input
            string[] userTokens = userInput.ToLower().Split(' ');
            string bestScript = "";
            int maxCount = 0;
            string unAnswered = @"I'm sorry, I didn't understand your question. Could you please rephrase it?";
            string referToAppointment = @"I apologize, I am unable to answer your question, 
                                  please use the appointment within gabay to book an
                                  appointment so that a representative can answer your question.";
            //the chatbot check first if the userinput is in english
            if (!IsEnglish(userInput))
            {
                bestScript = "I apologize, I am unable to understand any language. Please ask your question in English.";
            }
            else
            {
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
                    countUnAnswered++;

                    // if the ViewState counts is more than 3 it sets bestScript to referToAppointment otherwise bestScript is set to unAnswered
                    if (countUnAnswered >= 3)
                    {
                        bestScript = referToAppointment;
                        countUnAnswered = 0;
                    }
                    else
                    {
                        bestScript = unAnswered;
                    }
                    // Update the ViewState count
                    CountUnAnswered = countUnAnswered;
                }
                else
                {
                    bestScript = bestScript.Replace("\n", "<br>");
                    countUnAnswered = 0;
                    CountUnAnswered = countUnAnswered;
                }
            }
            

            return bestScript;
        }
        
        //function that handles the checking of langauge
        public bool IsEnglish(string input)
        {
            var identifier = languageIdentifierFactory.Load("Core14.profile.xml");

            var languages = identifier.Identify(input);
            var topLanguage = languages.FirstOrDefault();

            // Check if the top detected language is English
            return topLanguage?.Item1?.Iso639_2T == "eng";
        }
    }
}