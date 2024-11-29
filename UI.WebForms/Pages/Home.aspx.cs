using log4net;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UI.WebForms.ApiProxy;

namespace UI.WebForms.Pages
{
    public partial class Home : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Home));

        protected async void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    await LoadBooks();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Redirect("/Error");
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        private async Task LoadBooks()
        {
            var bookServiceCaller = Global.ServiceProvider.GetRequiredService<BookServiceCaller>();
            var books = await bookServiceCaller.GetAllBooksAsync();
            BooksGridView.DataSource = books;
            BooksGridView.DataBind();
        }
    }
}