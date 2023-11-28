﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/DashBoard/Guest_Homepage/Guest_Master.Master" AutoEventWireup="true" CodeBehind="Guest_Chatbot.aspx.cs" Inherits="Gabay_Final_V2.Views.Modules.Chatbot.Guest_Chatbot" EnableViewState="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../Resources/CustomStyleSheet/Chatbot/ChatbotStyle.css" rel="stylesheet" />
    <script src="../../../Resources/CustomJS/Chatbot/ChatbotJS.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" class="d-flex justify-content-center">
    <div class="chatbot-wrapper">
        <div class="chatbot-container">
            <div class="header custom-background custom-font">
                <span>Chat with Gab</span>
            </div>
            <div class="chatContainer" id="chatContainer" runat="server"></div>
            <div class="input-group userInput-group">
                <asp:TextBox ID="txtUserInput" runat="server" CssClass="form-control userInput" aria-describedby="btnSend" placeholder="Enter your message here..."></asp:TextBox>
                <asp:Button ID="btnSend" runat="server" CssClass="btn sndBtn" Text="Send" OnClick="btnSend_Click" />
            </div>
        </div>
    </div>
    <script>
        function menuButtonClick(choice) {
            document.getElementById('<%= txtUserInput.ClientID %>').value = choice;
            document.getElementById('<%= btnSend.ClientID %>').click();
        }
    </script>
</asp:Content>
