using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegrationTestingExample
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IntegrationTestingExample", Version = "v1" });
                c.AddSecurityDefinition("oauth2",
                   new OpenApiSecurityScheme()
                   {
                       Flows = new OpenApiOAuthFlows()
                       {
                           Implicit = new OpenApiOAuthFlow()
                           {
                               AuthorizationUrl = new Uri("https://login.microsoftonline.com/9afb6f1a-9a48-4d54-9549-8a99d4c686c2/oauth2/v2.0/authorize"),
                               TokenUrl = new Uri("https://login.microsoftonline.com/9afb6f1a-9a48-4d54-9549-8a99d4c686c2/oauth2/v2.0/token"),
                               Scopes = new Dictionary<string, string>()
                               {
                                   { "openid", "Open ID" },
                                   { "api://30827521-946b-4ce1-928c-169981998d48/graphapi", "Graph API" },
                                   { "offline_Access", "Offline Access" },
                               }
                           }
                       },
                       In = ParameterLocation.Header,
                       Name = "Authorization",
                       Type = SecuritySchemeType.OAuth2,
                   });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            },
                            Scheme = "oauth2",
                            Name = "oauth2",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
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
                app.UseSwaggerUI(c =>
                {
                    c.OAuthClientId(Configuration["AzureAd:ClientId"]);
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "IntegrationTestingExample v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
