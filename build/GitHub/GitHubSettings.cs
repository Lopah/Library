using System.Diagnostics.CodeAnalysis;

namespace Nuke.Build.Custom.GitHub;

[ExcludeFromCodeCoverage]
public class GitHubSettings
{
    public string GithubSource { get; init; }

    public string NugetOrgApiKey { get; init; }
}