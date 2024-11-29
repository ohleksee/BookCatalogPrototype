using log4net;
using Microsoft.Extensions.DependencyInjection;
using Models.Contracts;
using System;
using System.Threading.Tasks;
using UI.WebForms.ApiProxy;

namespace UI.WebForms.Pages
{
    public partial class EditBook : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EditBook));

        private int bookId = -1;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["id"], out bookId))
                bookId = -1;

            if (bookId == -1)
            {
                //maybe it make sense to redirect to resource not found page
                Response.Redirect("/", true);
                Context.ApplicationInstance.CompleteRequest();
            }

            try
            {
                if (!IsPostBack)
                {
                    await LoadCategories();
                    await LoadBookData();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Redirect("/Error");
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private async Task LoadCategories()
        {
            var categoryServiceCaller = Global.ServiceProvider.GetRequiredService<CategoryServiceCaller>();
            var categories = await categoryServiceCaller.GetAllCategoriesAsync();
            CategoryDropdown.DataSource = categories;
            CategoryDropdown.DataTextField = "Name";
            CategoryDropdown.DataValueField = "Id";
            CategoryDropdown.DataBind();
        }
        private async Task LoadBookData()
        {
            var bookServiceCaller = Global.ServiceProvider.GetRequiredService<BookServiceCaller>();
            var book = await bookServiceCaller.GetBookByIdAsync(bookId);
            if (book != null)
            {
                IdTextBox.Value = bookId.ToString();
                TitleTextBox.Text = book.Title;
                AuthorTextBox.Text = book.Author;
                ISBNTextBox.Text = book.ISBN;
                PublicationYearTextBox.Text = book.PublicationYear.ToString();
                QuantityTextBox.Text = book.Quantity.ToString();
                CategoryDropdown.SelectedValue = book.CategoryId.ToString();
            }
        }

        protected async void UpdateButton_Click(object sender, EventArgs e)
        {
            try
            {
                var book = new BookDto
                {
                    Id = bookId,
                    Title = TitleTextBox.Text,
                    Author = AuthorTextBox.Text,
                    ISBN = ISBNTextBox.Text,
                    PublicationYear = int.Parse(PublicationYearTextBox.Text),
                    Quantity = int.Parse(QuantityTextBox.Text),
                    CategoryId = int.Parse(CategoryDropdown.SelectedValue)
                };
                var bookServiceCaller = Global.ServiceProvider.GetRequiredService<BookServiceCaller>();
                await bookServiceCaller.UpdateBookAsync(bookId, book);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Redirect("/Error");
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            Response.Redirect("/", true);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}