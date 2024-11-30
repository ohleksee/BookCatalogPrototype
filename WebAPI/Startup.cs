using BookCatalog.DataAccess;
using BookCatalog.DataAccess.Repositories;
using BookCatalog.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics;

namespace WebAPI
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add DbContext
            services.AddDbContext<BookCatalogDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("bookCatalog")));

            // Register application services
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // Add Swagger for API documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookCatalog.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
           // if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookCatalog API v1"));
            }

            app.Use(async (context, next) =>
            {
                var requestId = context.Request.Headers["X-Request-ID"].ToString();
                if (string.IsNullOrEmpty(requestId))
                {
                    requestId = Guid.NewGuid().ToString();
                }
                context.Items["RequestId"] = requestId;
                logger.LogInformation($"Request ID: {requestId} - {context.Request.Method} {context.Request.Path}");

                // Add the Request ID to the response headers so it's accessible on the client side
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers["X-Request-ID"] = requestId;
                    return Task.CompletedTask;
                });

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    await next();
                    stopwatch.Stop();
                    logger.LogInformation($"Request ID: {requestId} - Response: {context.Response.StatusCode} (Execution time: {stopwatch.ElapsedMilliseconds} ms)");
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    logger.LogError($"Request ID: {requestId} - Error processing request: {ex.Message}. Execution Time: {stopwatch.ElapsedMilliseconds} ms");
                    throw;
                }
            });

            app.UseRouting();
            //app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}