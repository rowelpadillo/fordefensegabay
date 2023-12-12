<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm22.aspx.cs" Inherits="Gabay_Final_V2.Prototype.WebForm22" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta charset="UTF-8"/>
    <link href="../Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <link href="../Resources/CustomStyleSheet/Chatbot/ChatbotStyle.css" rel="stylesheet" />
    <script src="../Bootstrap/Scripts/bootstrap.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                <Columns>
                   
                </Columns>
            </asp:GridView>
            <div class="d-flex flex-column" style="width:300px;">
                <asp:TextBox ID="Response" runat="server" TextMode="MultiLine" style="height:80px;"></asp:TextBox>
                <asp:DropDownList ID="CategoryList" runat="server"></asp:DropDownList>
                <asp:TextBox ID="Keywords" runat="server"></asp:TextBox>
            </div>
            <asp:Button ID="Button1" runat="server" Text="Button" />
        </div>
    </form>
</body>
</html>
