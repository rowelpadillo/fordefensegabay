<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageStudent.aspx.cs" Inherits="Gabay_Final_V2.Prototype.WebForm3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Bootstrap/Content/bootstrap.css" rel="stylesheet" />
    <script src="../Bootstrap/Scripts/bootstrap.bundle.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by Name" AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" ></asp:TextBox>
            <asp:GridView ID="studentList" runat="server" CssClass="Table" AutoGenerateColumns="false">
               <Columns>
                   <asp:BoundField DataField="ex_ID" HeaderText="ID" />
                   <asp:BoundField DataField="Name" HeaderText="Name"/>
                   <asp:BoundField DataField="Address" HeaderText="Adress"/>
                   <asp:TemplateField>
                       <ItemTemplate>
                           <asp:LinkButton ID="linkBtnDelete" runat="server" OnClientClick='<%# "return showConfirmationModal(" + Eval("ex_ID") + ");" %>' Text="Delete"></asp:LinkButton>
                       </ItemTemplate>
                   </asp:TemplateField>
               </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hidPersonID" runat="server" />
        </div>
        <%-- confirmation modal --%>
        <div id="confirmationModal" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Confirmation</h5>
                       <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        Are you sure you want to delete this record?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <asp:Button ID="btnConfirmDelete" runat="server" Text="Confirm" OnClick="btnConfirmDelete_Click" CssClass="btn btn-danger" />
                    </div>
                </div>
            </div>
        </div>
        <%-- after deleted modal --%>
        <div class="modal fade" id="successModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-body bg-success text-center text-light">
                        <i class="bi bi-info-circle-fill"></i>
                       <span> Record Deleted</span>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        function showConfirmationModal(id) {
            // Store the ID in a hidden field or JavaScript variable to access it later in btnConfirmDelete_Click
            document.getElementById('<%= hidPersonID.ClientID %>').value = id;

            // Show the Bootstrap modal
            $('#confirmationModal').modal('show');

            // Prevent the postback
            return false;
        }
    </script>
</body>
</html>
