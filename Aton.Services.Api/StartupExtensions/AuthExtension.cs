using System.Security.Claims;
using Aton.Infrastructure.Identity.Data;
using Aton.Infrastructure.Identity.Managers;
using Aton.Services.Api.Auth;
using idunno.Authentication.Basic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Aton.Services.Api.StartupExtensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddCustomizedAuth(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();
            services.AddDbContext<AccountDbContext>(builder => builder.UseInMemoryDatabase("Aton"));
            services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
                .AddBasic(options =>
                {
                    options.SuppressWWWAuthenticateHeader = true;
                    options.AllowInsecureProtocol = true;
                    options.Realm = "basic";
                    options.Events = new BasicAuthenticationEvents
                    {
                        OnValidateCredentials = async context =>
                        {
                            var signInManager =
                                context.HttpContext.RequestServices.GetService<SignInManager>();
                            var accountManager =
                                context.HttpContext.RequestServices.GetService<AccountManager>();
                            
                            bool admin = false;
                            try
                            {
                                if (!signInManager.ValidateCredentials(context.Username, context.Password))
                                {
                                    context.Fail("Invalid credentials");
                                    return;
                                }

                                if (!await accountManager.IsActiveAsync(context.Username))
                                {
                                    context.Fail("Your account has been revoked");
                                    return;
                                }
                
                                admin = await accountManager.IsAdminAsync(context.Username);
                            }
                            catch (InvalidOperationException ex)
                            {
                                context.Fail(ex);
                                return;
                            }
                            
                            var claims = new List<Claim>()
                            {
                                new (
                                    ClaimTypes.Name, 
                                    context.Username, 
                                    ClaimValueTypes.String, 
                                    context.Options.ClaimsIssuer),
                            };
                            
                            if(admin)
                                claims.Add(new (
                                    ClaimTypes.Role, 
                                    "Admin", 
                                    ClaimValueTypes.String, 
                                    context.Options.ClaimsIssuer));

                            context.Principal = new ClaimsPrincipal(
                                new ClaimsIdentity(claims, context.Scheme.Name));
                            context.Success();
                        }
                    };
                });
            return services;
        }
        // public static IServiceCollection AddCustomizedAuth(this IServiceCollection services, IConfiguration configuration)
        // {
        //     var secretKey = configuration.GetValue<string>("SecretKey");
        //     var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        //
        //     services.AddIdentity<ApplicationUser, IdentityRole>()
        //         .AddRoles<IdentityRole>()
        //         .AddEntityFrameworkStores<AuthDbContext>()
        //         .AddDefaultTokenProviders();
        //
        //     var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));
        //
        //     services.Configure<JwtIssuerOptions>(options =>
        //     {
        //         options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        //         options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
        //         options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        //     });
        //
        //     var tokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
        //
        //         ValidateAudience = true,
        //         ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
        //
        //         ValidateIssuerSigningKey = true,
        //         IssuerSigningKey = _signingKey,
        //
        //         RequireExpirationTime = false,
        //         ValidateLifetime = true,
        //         ClockSkew = TimeSpan.Zero
        //     };
        //
        //     services.AddAuthentication(options =>
        //     {
        //         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //
        //     }).AddJwtBearer(configureOptions =>
        //     {
        //         configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        //         configureOptions.TokenValidationParameters = tokenValidationParameters;
        //         configureOptions.SaveToken = true;
        //     });
        //
        //
        //     services.AddAuthorization(options =>
        //     {
        //         var policy1 = new AuthorizationPolicyBuilder()
        //             .RequireAuthenticatedUser()
        //             .RequireRole("Admin")
        //             .AddRequirements(new ClaimRequirement("Customers_Write", "Write"))
        //             .Build();
        //         var policy2 = new AuthorizationPolicyBuilder()
        //             .RequireAuthenticatedUser()
        //             .RequireRole("Admin")
        //             .AddRequirements(new ClaimRequirement("Customers_Remove", "Remove"))
        //             .Build();
        //         options.AddPolicy("CanWriteCustomerData", policy1);
        //         options.AddPolicy("CanRemoveCustomerData", policy2);
        //     });
        //
        //     return services;
        // }
        //
        // public static IApplicationBuilder UseCustomizedAuth(this IApplicationBuilder app)
        // {
        //     app.UseAuthentication();
        //     app.UseAuthorization();
        //
        //     return app;
        // }
    }
}
