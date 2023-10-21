<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm16.aspx.cs" Inherits="Gabay_Final_V2.Prototype.WebForm16" EnableEventValidation="False" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <script src="../Bootstrap/Scripts/bootstrap.bundle.js"></script>
    <script src="../Resources/CustomJS/jquery-3.5.1.slim.min.js"></script>
    <script src="../Resources/CustomJS/bootstrap.min.js"></script>
    <link href="../Resources/CustomStyleSheet/DefaultStyle.css" rel="stylesheet" />
    <style>
        .appointeeName{
            color:#003366;
            font-weight:600;
        }
        .appointmentSchedule{
            color: white;
            border: 1px solid #dcdcdc;
            padding:10px;
            border-radius: 10px;
            background-color: #003366;
        }
        .appointmentStatus{
            border-radius: 15px;
            background-color: white;
            height:25px;
            color:#003366;
            padding: 0 8px;
        }
        .AppointmentActions a{
            text-decoration:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-lg-10 col-md-12">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control float-end mb-3" placeholder="Search student..."></asp:TextBox>
                </div>
                <div class="col-lg-2 col-md-12">
                    <div class="dropdown">
                        <button class="btn btn-secondary dropdown-toggle w-100" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                            Appointment Status
                        </button>
                        <ul class="dropdown-menu w-100">
                            <li>
                                <asp:LinkButton ID="displayPending" CssClass="dropdown-item" runat="server">Pending</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="displayActive" CssClass="dropdown-item" runat="server">Reschedule</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="displayDeactivated" CssClass="dropdown-item" runat="server">Approved</asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                </div>
               
                <div class="col-12">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table" DataKeyNames="ID_appointment" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="ID_appointment" HeaderText="Appointment ID" />
                            <asp:BoundField DataField="full_name" HeaderText="Recipient" />
                            <asp:BoundField DataField="role" HeaderText="User Type" />
                            <asp:BoundField DataField="appointment_date" HeaderText="Date"  DataFormatString="{0:dd MMM, yyyy}"/>
                            <asp:BoundField DataField="appointment_time" HeaderText="Time" />
                            <asp:BoundField DataField="appointment_status" HeaderText="Status" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="ViewConcernModal" runat="server" Text="Button" CssClass="btn bg-primary text-light" OnClick="ViewConcernModal_Click" OnClientClick='<%# "return getAppointmentID(" + Eval("ID_appointment") + ");" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="HiddenFieldAppointment" runat="server" />
            </div>
        </div>
        <div class="modal fade" id="exampleModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="exampleModalLabel">Appointment Content</h1>
                        <asp:Button ID="CloseViewModal" runat="server" CssClass="btn-close" OnClick="CloseViewModal_Click" OnClientClick="return true;" />
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row g-2">
                                <div class="col-12 mb-2">
                                    <div class="appointeeName">
                                        <asp:Label ID="appointmentName" runat="server" CssClass="fs-4"></asp:Label>
                                    </div>
                                    <div>
                                        <label for="Label1" class="text-secondary">Appointment ID: </label>
                                        <asp:Label ID="Label1" runat="server" CssClass="text-secondary"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-12 mb-2">
                                    <div class="form-floating">
                                        <asp:TextBox ID="appointmentConcern" CssClass="form-control" runat="server"  placeholder="Concern"  style="height: 100px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                        <label for="appointmentConcern">Concern</label>
                                    </div>
                                </div>
                                <div class="col-12 mb-2">
                                   <div class="appointmentSchedule d-flex flex-column">
                                       <div class="d-flex justify-content-between">
                                           <span class="fs-5 fw-medium">Schedule</span>
                                           <div class="appointmentStatus">
                                               <asp:Label ID="AppointmentStatus" runat="server" Text="Status"></asp:Label>
                                           </div>
                                       </div>
                                       <asp:Label ID="AppointmentDate" runat="server" Text="Date"></asp:Label>
                                       <asp:Label ID="AppointmentTime" runat="server" Text="Time"></asp:Label>
                                   </div>
                                </div>
                                <div class="col-12 mb-2">
                                    <div class="AppointmentActions">
                                        <asp:LinkButton ID="appointmentReschedule" runat="server" OnClick="appointmentReschedule_Click">
                                            <i class="bi bi-calendar-minus-fill"></i>
                                            <span>Reschedule Appointment</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="col-8 mb-2 d-grid">
                                    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                                        Launch static backdrop modal
                                    </button>
                                </div>
                                 <div class="col-4 mb-2 d-grid">
                                    <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                                        Reject
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="modal fade" id="reschedModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        ...
                     ...
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-primary">Save changes</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
        <!-- Bootstrap JS and jQuery -->
    <script>
        function getAppointmentID(id) {
            document.getElementById('<%= HiddenFieldAppointment.ClientID %>').value = id;
        }
    </script>
</body>
</html>


