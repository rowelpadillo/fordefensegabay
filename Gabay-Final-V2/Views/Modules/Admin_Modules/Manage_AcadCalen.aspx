<%@ Page Title="" Language="C#" MasterPageFile="~/Views/DashBoard/Admin_Homepage/Admin_Master.Master" AutoEventWireup="true" CodeBehind="Manage_AcadCalen.aspx.cs" Inherits="Gabay_Final_V2.Views.Modules.Admin_Modules.Manage_AcadCalen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../../Resources/CustomJS/OpenModal.js"></script>
    <script src="../../../Resources/CustomJS/AcadCalendar/AcadCalendarJS.js"></script>
    <link href="../../../Resources/CustomStyleSheet/Acad_Calendar/AcadCalenStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <span class="fs-2">Manage Academic Calendar</span>
            </div>
            <div class="col-12">
                <div class="row">
                    <div class="col-lg-4 col-md-12">
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="fileUpload form-control mb-3" />
                        <div class="FileError text-danger d-none" id="FileError">
                            <span><i class="bi bi-info-circle"></i></span>
                            <span>Please provide pdf file</span>
                        </div>
                        <asp:TextBox ID="fileName" runat="server" placeholder="Input School Year" CssClass="fileName form-control mb-3"></asp:TextBox>
                        <div class="nameError text-danger d-none" id="nameError">
                            <span><i class="bi bi-info-circle"></i></span>
                            <span>Please provide school year</span>
                        </div>
                        <div class="d-grid">
                            <asp:Button ID="upldBtn" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="upldBtn_Click" />
                        </div>
                    </div>
                    <div class="col-lg-8 col-md-12">
                        <div class="uploaded_container">
                            <asp:GridView ID="upldTbl" runat="server" CssClass="table table-striped fileTable" HeaderStyle-CssClass="text-center sticky-top" AlternatingRowStyle-BackColor="#c1d6fe" AutoGenerateColumns="False" DataKeyNames="FileId" OnRowEditing="upldTbl_RowEditing" OnRowDeleting="upldTbl_RowDeleting">
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="File ID" ItemStyle-Width="80px" ItemStyle-CssClass="">
                                        <ItemTemplate>
                                            <asp:Label ID="FileId" runat="server" Text='<%# Bind("FileId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="File Name" ItemStyle-CssClass="cntrlItm">
                                        <ItemTemplate>
                                            <asp:Label ID="FileName" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="cntrlItm" ControlStyle-CssClass="btn cntrlBtn">
                                        <ItemTemplate>
                                            <asp:Button ID="DeleteButton" runat="server" Text="Delete" CommandName="Delete" UseSubmitBehavior="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <%-- MODAL success --%>
            <div class="modal fade" id="myModal">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <!-- Placeholder for content -->
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
