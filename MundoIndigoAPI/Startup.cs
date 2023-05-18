using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistencia;
using Persistencia.Interfaces;
using System.Data;
using System.Text;

namespace WebApi
{
    public class Startup
    {
        private readonly string MyCors = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IReadDbContext, ReadDbContext>();
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(Configuration["ConnectionStrings:AppConexion"]));

            var appSettingsSection = Configuration.GetSection("App");
            services.Configure<App>(appSettingsSection);
            var appSettings = appSettingsSection.Get<App>();

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));

            services.AddAuthentication(x =>
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
            services.AddDbContext<AplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AppConexion")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            var urlPermitidas = Configuration.GetSection("App:UrlPermitidas").Get<List<string>>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyCors, builder =>
                {
                    //builder.WithOrigins("www.mundoIndigo.com");
                    //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                    //.AllowAnyHeader().AllowAnyMethod();

                    builder.WithOrigins(urlPermitidas.ToArray())
                                       .AllowAnyHeader()
                                       .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseRouting();

            app.UseCors(MyCors);

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
