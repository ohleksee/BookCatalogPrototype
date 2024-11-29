<%@ Page Title="Book List" Language="C#" Async="true" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Home.aspx.cs" Inherits="UI.WebForms.Pages.Home" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1>Books</h1>
        </section>

        <div class="row">
            <asp:GridView ID="BooksGridView" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Author" HeaderText="Author" />
                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                    <asp:BoundField DataField="PublicationYear" HeaderText="Publication Year" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="/EditBook?id={0}" Text="Edit" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="/DeleteBook?id={0}" Text="Delete" />
                </Columns>
            </asp:GridView>
        </div>

        <div class="row">
            <a href="/AddBook">Add New Book</a>
        </div>
    </main>
</asp:Content>
