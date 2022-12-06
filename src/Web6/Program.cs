using Microsoft.EntityFrameworkCore;
using Web6.Data;
using Magicodes.DynamicSqlApi.All;
using Magicodes.DynamicSqlApi.Core;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((hostingContext, config) => {
    var env = hostingContext.HostingEnvironment;
    //���ݻ����������ز�ͬ��JSON����
    config.AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
            true, true);
    //�ӻ��������������
    config.AddEnvironmentVariables();
    config.AddXmlFile("sqlMapper.xml", true, false);
});

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAllDynamicSqlApi(connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDynamicSqlApi();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.Run();
