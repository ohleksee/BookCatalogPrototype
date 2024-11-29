<%@ Page Title="Delete Book" Async="true" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="DeleteBook.aspx.cs" Inherits="UI.WebForms.Pages.DeleteBook" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1>Book delete confirmation</h1>
        </section>

        <div class="row">
            You're about to delete the following book, please confirm.            
        </div>

        <div class="row">
            <asp:HiddenField ID="IdTextBox" runat="server" />
            <asp:TextBox ID="TitleTextBox" runat="server" Placeholder="Title" ReadOnly="true" />
            <asp:TextBox ID="AuthorTextBox" runat="server" Placeholder="Author" ReadOnly="true" />
            <asp:TextBox ID="ISBNTextBox" runat="server" Placeholder="ISBN" ReadOnly="true" />
            <asp:TextBox ID="PublicationYearTextBox" runat="server" Placeholder="Publication Year" ReadOnly="true" />
            <asp:TextBox ID="QuantityTextBox" runat="server" Placeholder="Quantity" ReadOnly="true" />
            <asp:DropDownList ID="CategoryDropdown" runat="server" ReadOnly="true" />
            <asp:Button ID="DeleteButton" runat="server" Text="Confirm" OnClick="DeleteButton_Click" />
        </div>

        <div class="row">
            <a href="/">Return to list</a>
        </div>
    </main>
</asp:Content>
