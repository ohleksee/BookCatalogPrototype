<%@ Page Title="Add New Book" Async="true" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AddBook.aspx.cs" Inherits="UI.WebForms.Pages.AddBook" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1>Add Book</h1>
        </section>

        <div class="row">
            <asp:TextBox ID="TitleTextBox" runat="server" Placeholder="Title" />
            <asp:TextBox ID="AuthorTextBox" runat="server" Placeholder="Author" />
            <asp:TextBox ID="ISBNTextBox" runat="server" Placeholder="ISBN" />
            <asp:TextBox ID="PublicationYearTextBox" runat="server" Placeholder="Publication Year" />
            <asp:TextBox ID="QuantityTextBox" runat="server" Placeholder="Quantity" />
            <asp:DropDownList ID="CategoryDropdown" runat="server" />
            <asp:Button ID="AddButton" runat="server" Text="Add" OnClick="AddButton_Click" />
        </div>

        <div class="row">
            <a href="/">Back to the list</a>
        </div>
    </main>
</asp:Content>

