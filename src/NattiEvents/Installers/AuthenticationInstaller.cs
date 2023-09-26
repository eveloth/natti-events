using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using NattiEvents.Options;

namespace NattiEvents.Installers;

public static class AuthenticationInstaller
{
    public static void InstallAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<GitHubOauthOptions>()
            .Bind(builder.Configuration.GetSection(GitHubOauthOptions.GitHub));

        var gitHubOauthOptions = builder.Configuration
            .GetSection(GitHubOauthOptions.GitHub)
            .Get<GitHubOauthOptions>();

        ArgumentNullException.ThrowIfNull(gitHubOauthOptions);

        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Lax;

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                }
            )
            .AddGitHub(
                GitHubAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.UsePkce = true;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    options.ClientId = gitHubOauthOptions.ClientId;
                    options.ClientSecret = gitHubOauthOptions.ClientSecret;
                    options.CallbackPath = gitHubOauthOptions.CallbackPath;

                    options.SaveTokens = true;

                    options.Events.OnCreatingTicket = async context => await SyncUser(context);
                }
            );
    }

    private static async Task SyncUser(OAuthCreatingTicketContext context) =>
        throw new NotImplementedException();
}
