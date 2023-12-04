﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/DashBoard/Admin_Homepage/Admin_Master.Master" AutoEventWireup="true" CodeBehind="Manage_CampusInfo.aspx.cs" Inherits="Gabay_Final_V2.Views.Modules.Admin_Modules.Manage_CampusInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 style="text-align: center; padding: 9px; border: 2px solid #333; background-color: #f4f4f4; color: #333; border-radius: 10px;">Manage Campus Information</h1>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
<%--    <div class="container">--%>
            <div class="accordion">
                        <div class="accordion-image-container">
                          <img src="../../../Resources/Images/uclm.png" alt="Accordion Image" class="accordion-image" />
                        </div>
                <asp:Repeater ID="rptAccordion" runat="server">
                <ItemTemplate>
                        <div class="accordion-content" data-item-id='<%# Eval("id") %>' data-item-title='<%# Eval("Title") %>'>
                            <header>
                                <span class="title"><%# Eval("Title") %></span>
                                <i class="fa-solid fa-plus"></i>
                            </header>
                            <p class="description">
                                <br /><br /><%# Eval("Content") %><br /><br />
                            <button type="button" class="edit-button" data-toggle="modal" data-target="#editModal"
                                onclick="editAccordionItem('<%# Eval("id") %>','<%# Eval("Title") %>','<%# Eval("Content") %>')">
                                <i class="fas fa-pencil-alt"></i>
                            </button>
                            </p>
                        </div>
             </ItemTemplate>
            </asp:Repeater>
<%--          </div>--%>
        </div>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
     <asp:Literal ID="accordionContainer" runat="server"></asp:Literal>
<!-- Modal -->
<div class="modal fade" id="editModal" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="editModalLabel">Edit Content</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <!-- Add the form elements for editing content here -->
                <label for="editTitle">Title:</label>
                <asp:TextBox ID="txtNewTitle" runat="server" CssClass="form-control"></asp:TextBox>
                <br />
              <label for="editContent">Content:</label>
               <asp:TextBox ID="txtNewContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="reloadPage()">Close</button>
                <asp:Button ID="btnSaveChanges" runat="server" Text="Save changes" CssClass="btn btn-primary" OnClick="btnSaveChanges_Click" />       
                <asp:Button ID="btnDeleteContent" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="btnDeleteContent_Click" />
                <asp:Button ID="btnAddNewRecord" runat="server" Text="Add New" CssClass="btn btn-success" OnClick="btnAddNewRecord_Click" />
            </div>
        </div>
    </div>
</div>
 <asp:HiddenField ID="hdnAccordionContent" runat="server" />
 <asp:HiddenField ID="hdnAccordionIndex" runat="server" />
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const accordionContent = document.querySelectorAll(".accordion-content");

            accordionContent.forEach((item, index) => {
                let header = item.querySelector("header");
                header.addEventListener("click", () => {
                    item.classList.toggle("open");

                    let description = item.querySelector(".description");
                    if (item.classList.contains("open")) {
                        description.style.height = "auto";
                        item.querySelector("i").classList.replace("fa-plus", "fa-minus");
                    } else {
                        description.style.height = "0";
                        item.querySelector("i").classList.replace("fa-minus", "fa-plus");
                    }
                    removeOpen(index);
                });
            });

            function removeOpen(index1) {
                accordionContent.forEach((item2, index2) => {
                    if (index1 != index2) {
                        item2.classList.remove("open");

                        let des = item2.querySelector(".description");
                        des.style.height = "0";
                        item2.querySelector("i").classList.replace("fa-minus", "fa-plus");
                    }
                });
            }

            // Function to store the accordion content in the hidden field
            function storeAccordionContent() {
                const accordionContent = document.querySelectorAll(".accordion-content");
                const accordionData = [];

                accordionContent.forEach((item, index) => {
                    let title = item.querySelector(".title").innerText.trim();
                    let description = item.querySelector(".description").innerText.trim();

                    accordionData.push({ title: title, description: description, id: index });
                });

                document.getElementById('<%= hdnAccordionContent.ClientID %>').value = JSON.stringify(accordionData);
            }

            // Call the function to store the accordion content when the page loads
            window.onload = storeAccordionContent;

            function editAccordionItem(itemId) {
                // Assuming you have a hidden field with ID 'hdnAccordionIndex' to store the item ID for the update
                document.getElementById('hdnAccordionIndex').value = itemId;

                // Fetch the content from the accordion item and set it in the modal inputs
                var accordionItem = document.querySelector(`[data-item-id='${itemId}']`);
                var title = accordionItem.querySelector('.title').innerText.trim();
                var description = accordionItem.querySelector('.description').innerText.trim();

                document.getElementById('<%= txtNewTitle.ClientID %>').value = title;
                document.getElementById('<%= txtNewContent.ClientID %>').value = description;
            }
        });
    </script>
 <script>
     // Your JavaScript code for the accordion and modal goes here
     // ...
     // Use JSON.parse to convert the JSON data from the accordionContainer to a JavaScript array of objects
     var accordionData = JSON.parse('<%= accordionContainer.Text %>');

     // Function to create an accordion item based on the data
     function createAccordionItem(data) {
         return `
        <div class='accordion-content'>
            <header>
                <span class='title'>${data.Title}</span>
                <i class='fa-solid fa-plus'></i>
            </header>
            <p class='description'>
                <br /><br />${data.Description}<br /><br />
                <button type='button' class='edit-button' data-toggle='modal' data-target='#editModal' onclick='editAccordionItem(${data.id})'>Edit</button>
            </p>
        </div>`;
     }

     // Function to add all accordion items to the accordion container
     function populateAccordion() {
         var accordionContainer = document.querySelector('.accordion');

         accordionData.forEach(function (item) {
             var accordionItemHtml = createAccordionItem(item);
             accordionContainer.insertAdjacentHTML('beforeend', accordionItemHtml);
         });
     }

     // Call the function to populate the accordion when the page loads
     window.onload = populateAccordion;
     function reloadPage() {
         location.reload();
     }
 </script>
</asp:Content>
