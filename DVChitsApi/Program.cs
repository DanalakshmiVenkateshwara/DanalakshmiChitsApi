using Autofac.Core;
using BusinessManagers.Interfaces;
using BusinessManagers.Managers;
using DataAccess;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories;
using DataAccess.Repositories.Managers;
using System.Data.Common;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddCors(o => o.AddPolicy("DanalakshmiChitsCors", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddSingleton<IConnectionFactory,ConnectionFactory>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IRepository,RepositoryBase>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IAdminManger,AdminManger>();
builder.Services.AddScoped<IAdminRepository,AdminRepository>();

DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    //app.UseSwagger();
    //app.UseSwaggerUI();

}
else
    app.UseCors("DanalakshmiChitsCors");

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = String.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

