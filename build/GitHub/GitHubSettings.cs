using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Nuke.Build.Custom.GitHub;

[ExcludeFromCodeCoverage]
public class GitHubSettings
{
    public string GithubSource { get; [UsedImplicitly] init; }

    public string GithubAccessToken { get; [UsedImplicitly] init; }
}