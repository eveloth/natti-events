namespace NattiEvents.Options;

public class GitHubOauthOptions
{
    public const string GitHub = "GitHub";

    public string CallbackPath { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}
