<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pending_page.aspx.cs" Inherits="Gabay_Final_V2.Views.RegistrationPages.Pending_page" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pending Page</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link href="../../Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <link href="../../Resources/CustomStyleSheet/PendingPageStyle.css" rel="stylesheet" />
    <link href="../../Resources/CustomStyleSheet/DefaultStyle.css" rel="stylesheet" />
</head>
<body>
    <div class="container-fluid">
        <form id="form1" runat="server" class="row">
            <div class="col-12">
                <div class="container content-container">
                    <img src="../../Resources/Images/identification-card.png" class="imgID img-fluid" />
                    <h1 class="text-light">Account Verification in Progress</h1>
                    <div class="container mt-3">
                        <span class="text-light">We're verifying your account details.
                            Our team is reviewing the information for accuracy and security.
                            Thank you for your patience. We'll email you once verification is complete.
                            Keep an eye on your inbox for updates.</span>
                        <br />
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn gBck-Btn mt-3" NavigateUrl="~/Views/LoginPages/Student_login.aspx">Go back to Login</asp:HyperLink>
                    </div>
                </div>
            </div>
        </form>
    </div>
</body>
</html>
