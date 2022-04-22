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
        string username = null;
        bool admin = false;
        try  
        {  
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);  
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');  
            username = credentials.FirstOrDefault();  
            var password = credentials.LastOrDefault();  
  
            if (!_signInManager.ValidateCredentials(username, password))  
                throw new ArgumentException("Invalid credentials");
            admin = await _accountManager.IsAdminAsync(username);
        }  
        catch (Exception ex)  
        {  
            return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");  
        }  
  
        var claims = new List<Claim> {  
            new(ClaimTypes.Name, username),
        };  
        
        if(admin)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        
        var identity = new ClaimsIdentity(claims, Scheme.Name);  
        var principal = new ClaimsPrincipal(identity);  
        var ticket = new AuthenticationTicket(principal, Scheme.Name);  
  
        return AuthenticateResult.Success(ticket);  
    }  
  
}  