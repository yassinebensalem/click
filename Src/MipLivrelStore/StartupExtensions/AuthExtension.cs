using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DDD.Domain.Models;
using DDD.Infra.CrossCutting.Identity.Authorization;
using DDD.Infra.CrossCutting.Identity.Models;
using DDD.Infra.Data.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DDD.Services.Api.StartupExtensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddCustomizedAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var secretKey = configuration.GetValue<string>("SecretKey");
            var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            //    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
            //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //})
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(options =>
                {
                    options.AppId = configuration.GetValue<string>("Authentication:Facebook:AppId");
                    options.AppSecret = configuration.GetValue<string>("Authentication:Facebook:AppSecret");
                    options.AccessDeniedPath = "/AccessDeniedPathInfo"; 
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration.GetValue<string>("Authentication:Google:ClientId");
                    options.ClientSecret = configuration.GetValue<string>("Authentication:Google:ClientSecret");
                    options.AccessDeniedPath = "/AccessDeniedPathInfo";
                    options.ClaimActions.MapJsonKey("image", "picture");
                })
               .AddJwtBearer("Bearer", configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            }).AddCookie("Cookies", x =>
            {
                x.LoginPath = "/Login";
                x.LogoutPath = "/Logout";
                x.ExpireTimeSpan = TimeSpan.FromMinutes(120);
            });

            services.AddAuthorization();
            //services.AddAuthorization(options =>
            //{
            //    var policy1 = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .RequireRole("Admin")
            //        .AddRequirements(new ClaimRequirement("Customers_Write", "Write"))
            //        .Build();
            //    var policy2 = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .RequireRole("Admin")
            //        .AddRequirements(new ClaimRequirement("Customers_Remove", "Remove"))
            //        .Build();
            //    options.AddPolicy("CanWriteCustomerData", policy1);
            //    options.AddPolicy("CanRemoveCustomerData", policy2);
            //});

            return services;
        }

        public static IApplicationBuilder UseCustomizedAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
