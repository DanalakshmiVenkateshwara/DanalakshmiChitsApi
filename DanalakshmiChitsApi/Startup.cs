using Autofac;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DanalakshmiChitsApi.App_Start;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace DanalakshmiChitsApi
{
    public class Startup
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
            //services.AddSwaggerGen();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DanalakshmiChitsApi", Version = "v1" });
            });
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddCors(o => o.AddPolicy("DanalakshmiChitsCors", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseWebSockets();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                //app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DanalakshmiChitsApi v1"));
            }
            else
                app.UseCors("DanalakshmiChitsCors");

            app.UseHttpsRedirection();

            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

            //app.UseRouting();
            //app.UseCors("DanalakshmiChitsCors");
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsSelf().AsImplementedInterfaces();
            builder.RegisterModule(new AutofacModule());
        }

    }
}
