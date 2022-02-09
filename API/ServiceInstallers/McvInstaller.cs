using AutoMapper;
using Totem.API.Filters;
using Totem.API.ServiceInstallers.Base;
using Totem.Business.Core;
using Totem.Business.Core.AppSettings;
using Totem.Business.Core.DataTransferModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Totem.API.ServiceInstallers
{
    public class McvInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            IMvcBuilder mvcBuilder = services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Totem Api",
                    Description = "Web Api for Totem"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                 Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            #endregion

            #region Security

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("AppSettings:JWT:Issuer").Value,
                    ValidAudience = configuration.GetSection("AppSettings:JWT:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:JWT:Token").Value)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            #endregion

            #region CORS

            services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentCorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            #endregion

            services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)))
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        ModelStateDictionary modelStateDictionry = actionContext.ModelState;
                        return new BadRequestObjectResult(FormatOutput(modelStateDictionry));
                    };
                });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        /// <summary>
        /// Get errors list as <see cref="List{String}"/> from <see cref="ModelStateDictionary"/>
        /// </summary>
        private static ExecutionResult FormatOutput(ModelStateDictionary modelStateDictionry)
        {
            List<ErrorInfo> errors = new List<ErrorInfo>();

            modelStateDictionry.Values.ToList().ForEach(modelState =>
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errors.Add(new ErrorInfo
                    {
                        ErrorMessage = error.ErrorMessage
                    });
                }
            });

            return new ExecutionResult(errors);
        }
    }
}
