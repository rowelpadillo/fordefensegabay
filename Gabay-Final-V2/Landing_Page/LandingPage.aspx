<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="Gabay_Final_V2.Landing_Page.LandingPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <title>Landing Page</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <link href="../Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <link href="../Resources/CustomStyleSheet/DefaultStyle.css" rel="stylesheet" />
    <link href="../Resources/CustomStyleSheet/Landing_Page/LandingPageStyle.css" rel="stylesheet" />
    <link href="../FontAwesome/css/all.css" rel="stylesheet" />
    <script src="../Bootstrap/Scripts/bootstrap.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="../FontAwesome/js/all.js"></script>
</head>
<body>
    <nav class="navbar sticky-top navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">Gabay</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse flex-row-reverse" id="navbarNavAltMarkup">
                <div class="navbar-nav text-center">
                    <a class="nav-link" href="#homeSection">Home</a>
                    <a class="nav-link" href="#featureSection">Features</a>
                    <a class="nav-link" href="#">About us</a>
                    <button class="btn bg-primary" data-toggle="modal" data-target="#loginSelection">Get Started</button>
                </div>
            </div>
            
        </div>
    </nav>
    <div class="modal fade" id="loginSelection" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Login as</h5>
                </div>
                <div class="modal-body">
                    <!-- Cards inside the modal, horizontally aligned -->
                    <div class="row">
                        <div class="col-4">
                            <div class="card" style="width: auto;">
                                <h5 class="card-title text-center mt-1">Walk-in<br />
                                    Guest</h5>
                                <img src="../../Resources/Images/walk.gif" />
                                <div class="card-body">
                                    <p class="card-text">Explore our university as a walk-in guest and learn about admissions and tours using our kiosk.</p>
                                </div>
                                <asp:HyperLink CssClass="btn btn-outline-primary" ID="HyperLink1" runat="server" NavigateUrl="~/Views/LoginPages/Guest_login.aspx">Proceed</asp:HyperLink>

                            </div>
                        </div>
                        <div class="col-4">
                            <div class="card" style="width: auto;">
                                <h5 class="card-title text-center mt-1">Old/New<br />
                                    Students</h5>
                                <img src="../../Resources/Images/id.gif" />
                                <div class="card-body">
                                    <p class="card-text">For returning students and newcomers, discover the university's offerings and resources.</p>
                                </div>
                                <asp:HyperLink CssClass="btn btn-outline-primary" ID="btnStdnt" runat="server" NavigateUrl="~/Views/LoginPages/Student_login.aspx">Proceed</asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="card" style="width: auto;">
                                <h5 class="card-title text-center mt-1">Department<br />
                                    Personnel</h5>
                                <img src="../../Resources/Images/school.gif" />
                                <div class="card-body">
                                    <p class="card-text">Manage department's facilities, faculty, and programs for a fulfilling academic journey.</p>
                                </div>
                                <asp:HyperLink ID="HyperLink2" runat="server" CssClass="btn btn-outline-primary" NavigateUrl="~/Views/LoginPages/Department_login.aspx">Proceed</asp:HyperLink>
                                <%--<a href="#" class="btn btn-outline-primary">Card link</a>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <section id="homeSection"> 
        <div class="container-fluid homeSection">
            <div class="row h-100" >
                <div class="col-lg-5 col-md-12">
                    <div class="hLeft-cont">
                        <div class="hLeft">
                            <h1 class="titleHeader">A Web-based Information Kiosk</h1>
                            <div class="divider"></div>
                            <h3 class="text-secondary">Modern Approach to University Information Sharing</h3>
                        </div>
                        <div class="button-area mt-3">
                            <a class="btn exploreBtn" href="#featureSection">Start Exploring</a>
                        </div>
                    </div>
                </div>
                <div class="col-7">
                </div>
            </div>
        </div>
    </section>
    <section  id="featureSection">
        <div class="container-fluid featSection"">
            <div class="row">
                <div class="col-12 featHeader">
                    <span>FEATURES</span>
                </div>
                
            </div>
        </div>
    </section>
</body>
</html>
