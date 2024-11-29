using log4net;
using Microsoft.Extensions.DependencyInjection;
using Models.Contracts;
using System;
using System.Threading.Tasks;
using UI.WebForms.ApiProxy;

namespace UI.WebForms.Pages
{
    public partial class AddBook : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AddBook));
        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    await LoadCategories();
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

        protected async void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                var newBook = new BookDto
                {
                    Title = TitleTextBox.Text,
                    Author = AuthorTextBox.Text,
                    ISBN = ISBNTextBox.Text,
                    PublicationYear = int.Parse(PublicationYearTextBox.Text),
                    Quantity = int.Parse(QuantityTextBox.Text),
                    CategoryId = int.Parse(CategoryDropdown.SelectedValue)
                };

                var bookServiceCaller = Global.ServiceProvider.GetRequiredService<BookServiceCaller>();
                await bookServiceCaller.AddBookAsync(newBook);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Redirect("/Error");                
                return;
            }
            Response.Redirect("/", true);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}