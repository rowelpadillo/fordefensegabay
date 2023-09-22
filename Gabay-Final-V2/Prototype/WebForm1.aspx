<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Gabay_Final_V2.Prototype.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <link href="../Resources/CustomStyleSheet/DefaultStyle.css" rel="stylesheet" />
    <link href="../FontAwesome/css/all.css" rel="stylesheet" />
    <link href="../Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <link href="../Resources/CustomStyleSheet/Test.css" rel="stylesheet" />
    <script src="../Bootstrap/Scripts/bootstrap.bundle.js"></script>
    <script src="../FontAwesome/js/all.js"></script>
</head>
<body>
    <form id="form1" runat="server">
       <div class="container-fluid chatbot-container">
           <div class="header">
               <span>Chat with Gab</span>
           </div>
           <div class="chat-container">
               <div class="message-container bot-message">Hello! How can I assist you today?</div>
               <div class="message-container user-message">Hi, How to enroll in University of Cebu?</div>
           </div>
           <div class="input-group userInput">
               <asp:TextBox ID="userInput" runat="server" CssClass="form-control" aria-describedby="sndBtn" placeholder="Enter your message here..."></asp:TextBox>
               <asp:Button ID="sndBtn" runat="server" Text="Send" CssClass="btn btn-outline-secondary" UseSubmitBehavior="false"/>
           </div>
       </div>
    </form>
</body>
</html>
