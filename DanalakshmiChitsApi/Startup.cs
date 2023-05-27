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
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading;

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PracticeSol", Version = "v1" });
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                //app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PracticeSol v1"));
            }
            else
                app.UseCors("DanalakshmiChitsCors");

            app.UseHttpsRedirection();

            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

            //app.UseRouting();
            //app.UseCors("DanalakshmiChitsCors");
            //app.UseAuthorization();

            app.UseWebSockets();
            var wsOptions = new WebSocketOptions { KeepAliveInterval = TimeSpan.FromSeconds(120) };
            app.UseWebSockets(wsOptions);
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/send")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                        {
                            await Send(context, webSocket);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task Send(HttpContext context, WebSocket webSocket)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), System.Threading.CancellationToken.None);
                while (result != null)
                {
                    string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                    await webSocket.SendAsync(Encoding.ASCII.GetBytes($"Hi {msg} - {DateTime.Now}"), WebSocketMessageType.Text, true, CancellationToken.None);
                    await Task.Delay(1000);
                }
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsSelf().AsImplementedInterfaces();
            builder.RegisterModule(new AutofacModule());
        }

    }
}
