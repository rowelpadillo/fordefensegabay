<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm16.aspx.cs" Inherits="Gabay_Final_V2.Prototype.WebForm16" EnableEventValidation="False" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <script src="../Bootstrap/Scripts/bootstrap.bundle.js"></script>
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
                            <asp:BoundField DataField="appointment_date" HeaderText="Date"  DataFormatString="{0:MMMM-dd-yyyy}"/>
                            <asp:BoundField DataField="appointment_time" HeaderText="Time" />
                            <asp:BoundField DataField="appointment_status" HeaderText="Status" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="ViewAppointmentModal" CssClass="btn btn-primary" runat="server" OnClientClick='<%# "return getAppointmentID(" + Eval("ID_appointment") + ");" %>' OnClick="ViewAppointmentModal_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:HiddenField ID="HiddenFieldAppointment" runat="server" />
     
            </div>
        </div>
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="exampleModalLabel">Modal title</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-12 mb-2">
                                    <div class="form-floating mb-3">
                                        <%--<input type="email" class="form-control" id="appointmentName" placeholder="name@example.com" />--%>
                                        <asp:TextBox ID="appointmentName" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                                        <label for="appointmentName">Recipient</label>
                                    </div>
                                </div>
                                <div class="col-12 mb-2">
                                    <div class="form-floating">
                                        <asp:TextBox ID="appointmentConcern" CssClass="form-control" runat="server"  placeholder="Concern"  style="height: 100px"></asp:TextBox>
                                        <label for="appointmentConcern">Concern</label>
                                    </div>
                                </div>
                                <div class="col-6 mb-2">
                                    <div class="form-floating">
                                        <asp:DropDownList ID="AppointmentTime" runat="server" CssClass="form-select">
                                            <asp:ListItem Selected="True" Value="">
                                                Open this select menu
                                            </asp:ListItem>
                                            <asp:ListItem Value="9:00">9:00</asp:ListItem>
                                            <asp:ListItem Value="10:00">10:00</asp:ListItem>
                                            <asp:ListItem Value="11:00">11:00</asp:ListItem>
                                        </asp:DropDownList>
                                        <label for="AppointmentTime">Works with selects</label>
                                    </div>
                                </div>
                                <div class="col-6 mb-2">
                                    <div class="form-floating mb-3">
                                        <asp:TextBox ID="AppointmentDate" runat="server" CssClass="form-control" placeholder="Date" TextMode="Date"></asp:TextBox>
                                        <label for="AppointmentDate">Date</label>
                                    </div>
                                </div>
                            </div>
                        </div>
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
    <script src="../../../Scripts/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script>
        function showConfirmationModal(id) {
            // Store the ID in a hidden field or JavaScript variable to access it later in btnConfirmDelete_Click
            document.getElementById('<%= HiddenFieldAppointment.ClientID %>').value = id;
            // Show the Bootstrap modal
            /*$('#toDeleteModal').modal('show');*/
            // Prevent the postback
            return false;
        }
        function getAppointmentID(id) {
            document.getElementById('<%= HiddenFieldAppointment.ClientID %>').value = id;
        }
    </script>
</body>
</html>


