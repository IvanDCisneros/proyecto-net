using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Persistencia.Interfaces;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IReadDbContext, ReadDbContext>();
builder.Services.AddTransient<IDbConnection>((sp) => new SqlConnection(configuration["ConnectionStrings:AppConexion"]));

var appSettingsSection = configuration.GetSection("App");
builder.Services.Configure<App>(appSettingsSection);
var appSettings = appSettingsSection.Get<App>();

var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//services.AddDbContext<AplicationDBContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:AppConexion"]));
builder.Services.AddDbContext<AplicationDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppConexion")));
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
});

var urlPermitidas = configuration.GetSection("App:UrlPermitidas").Get<List<string>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.WithOrigins(urlPermitidas.ToArray()).AllowAnyOrigin()
            .AllowAnyHeader().AllowAnyMethod();

        //app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.Run();
