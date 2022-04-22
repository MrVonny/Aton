using Aton.Infrastructure.Identity.Data;
using Aton.Services.Api.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Aton.Services.Api.StartupExtensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddCustomizedAuth(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AccountDbContext>(builder => builder.UseInMemoryDatabase("Aton"));
            services.AddAuthentication("BasicAuthentication")  
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
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
