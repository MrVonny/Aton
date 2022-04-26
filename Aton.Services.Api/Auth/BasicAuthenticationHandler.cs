using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Aton.Infrastructure.Identity.Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Aton.Services.Api.Auth;
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>  
{  
    #region Property  
    readonly SignInManager _signInManager;
    private readonly AccountManager _accountManager;
    protected string FailureReason { get; set; }
    #endregion  
 
    #region Constructor  
    public BasicAuthenticationHandler(SignInManager signInManager,  
        IOptionsMonitor<AuthenticationSchemeOptions> options,  
        ILoggerFactory logger,  
        UrlEncoder encoder,  
        ISystemClock clock, AccountManager accountManager)  
        : base(options, logger, encoder, clock)
    {
        _signInManager = signInManager;
        _accountManager = accountManager;
    }  
    #endregion  
  
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()  
    {
        string login = null;
        bool admin = false;
        try
        {
            if (!Request.Headers["Authorization"].Any())
                throw new InvalidOperationException("Authorization header missing");
            
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);  
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');  
            login = credentials.FirstOrDefault();  
            var password = credentials.LastOrDefault();

            if (!_signInManager.ValidateCredentials(login, password))
            {
                throw new InvalidOperationException("Invalid credentials");
            }

            if (!await _accountManager.IsActiveAsync(login))
            {
                throw new InvalidOperationException("Your account has been revoked");
            }
                
            admin = await _accountManager.IsAdminAsync(login);
        }
        catch (InvalidOperationException ex)
        {
            FailureReason = ex.Message;
            return AuthenticateResult.Fail(FailureReason);  
        }

        var claims = new List<Claim> {  
            new(ClaimTypes.Name, login),
        };  
        
        if(admin)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        
        var identity = new ClaimsIdentity(claims, Scheme.Name);  
        var principal = new ClaimsPrincipal(identity);  
        var ticket = new AuthenticationTicket(principal, Scheme.Name);  
  
        return AuthenticateResult.Success(ticket);  
    }

    // protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    // {
    //     //Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //     var message = $"Authentication failed: {FailureReason}";
    //     await Context.Response.WriteAsync(message);
    // }
}  