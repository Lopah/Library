using System;
using System.Linq;
using Nuke.Build.Custom.Helpers;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Build.Custom.Paths;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Build.Custom.GitHub.Branch;

namespace Nuke.Build.Custom;

[GitHubActions("release-main", GitHubActionsImage.UbuntuLatest,
    OnPushBranches = new[] { MasterBranch, MainBranch, ReleaseBranchPrefix + "/*" },
    InvokedTargets = new[] { nameof(PublishGitHubRelease), nameof(Push) },
    ImportSecrets = new[] { nameof(PersonalAccessToken) },
    PublishArtifacts = true)]
[ShutdownDotNetAfterServerBuild]
public partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    static Environment _environment = Environment.Undefined;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution]
    readonly Solution Solution;

    [GitRepository]
    readonly GitRepository GitRepository;

    [GitVersion(NoFetch = true)]
    readonly GitVersion GitVersion;

    [Parameter("Environment to use for dotnet tasks")]
    public static Environment DotNetEnvironment { get; private set; } = Environment.Undefined;

    [Parameter]
    public static Environment Environment
    {
        get
        {
            if (!_environment.Equals(Environment.Undefined))
            {
                return _environment;
            }

            if (!DotNetEnvironment.Equals(Environment.Undefined))
            {
                return _environment;
            }

            return Environment.Development;
        }
        set
        {
            if (value is null)
            {
                _environment = Environment.Undefined;
                return;
            }

            _environment = value;
        }
    }

    [Parameter]
    [Secret]
    string PersonalAccessToken
    {
        get
        {
            if (string.IsNullOrEmpty(_githubAccessToken))
            {
                _githubAccessToken = Settings.GitHubSettings.GithubAccessToken;
            }

            return _githubAccessToken;
        }
        set => _githubAccessToken = value;
    }

    string _githubAccessToken;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProcessWorkingDirectory(SourceDirectory)
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            var projects = Solution.GetProjects("*.Application.*").ToList();

            Log.Information($"Found {projects.Count()} projects");
            if (projects is null)
            {
                throw new InvalidOperationException(
                    "Compilation of projects failed, could not find them. Verify the source directory pointer.");
            }

            projects.ForEach(project =>
            {
                DotNetBuild(s => s
                    .SetProcessWorkingDirectory(project.Directory)
                    .SetOutputDirectory(BinDirectory / project.Name)
                    .SetConfiguration(Configuration)
                    .SetAssemblyVersion(GitVersion.AssemblySemVer)
                    .SetFileVersion(GitVersion.AssemblySemFileVer)
                    .SetInformationalVersion(GitVersion.InformationalVersion)
                    .EnableNoRestore());
            });
        });


    Target UnitTests => _ => _
        .Description("Runs unit tests for the solution")
        .DependsOn(Compile)
        .Executes(() =>
        {
            TestsDirectory.GlobFiles("*.csproj").ForEach(testProject =>
            {
                DotNetTest(s => s
                    .SetProjectFile(testProject)
                    .SetConfiguration(Configuration.Debug));
            });
        });

    /// <summary>
    ///     If possible, don't release new version of library without testing client project that works with it
    /// </summary>
    Target Pack => _ => _
        .Description("Packs the shared library in this project to be then shared via NuGet")
        .DependsOn(Compile, Changelog)
        .DependsOn(UnitTests)
        .Executes(() =>
        {
            Solution.Projects.Where(e => e.IsPackable()).ForEach(e =>
            {
                DotNetPack(s => s
                    .SetProject(e)
                    .SetOutputDirectory(OutputDirectory)
                    .SetConfiguration(Configuration.Release)
                    .SetVersion(GitVersion.NuGetVersionV2)
                    .SetPackageReleaseNotes(SourceDirectory / "CHANGELOG.md")
                    .EnableNoRestore());
            });
        });

    T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}