﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nuke.Build.Custom.Components;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotCover;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using Nuke.GitHub;
using Serilog;
using static Nuke.CodeGeneration.CodeGenerator;
using static Nuke.Common.ChangeLog.ChangelogTasks;
using static Nuke.Common.Tools.DotCover.DotCoverTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.GitHub.GitHubTasks;
using static Nuke.Common.Tools.GitVersion.GitVersionTasks;
using static Nuke.Common.Utilities.StringExtensions;
using static Nuke.Build.Custom.Paths;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Build.Custom.GitHub.Branch;
using static Nuke.Common.IO.Globbing;


namespace Nuke.Build.Custom;

public partial class Build
{
    [Parameter]
    readonly bool AutoStash = true;

    string MajorMinorPatchVersion => GitVersion.MajorMinorPatch;

    Target Changelog => _ => _
        .Unlisted()
        .OnlyWhenStatic(() => Repository.IsOnReleaseBranch() || Repository.IsOnHotfixBranch())
        .Executes(() =>
        {
            var changeLogFile = From<IChangeLog>().ChangeLogFile;
            FinalizeChangelog(changeLogFile, MajorMinorPatchVersion, Repository);
            Log.Information("Please review CHANGELOG.md and press any key to continue...");
            Console.ReadKey();

            Git($"add {changeLogFile}");
            Git($"commit -m \"Finalize {Path.GetFileName(changeLogFile)} for {MajorMinorPatchVersion}\"");
        });


    [PublicAPI]
    Target Release => _ => _
        .DependsOn(Changelog)
        .Requires(() => !Repository.IsOnReleaseBranch() || GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            if (!Repository.IsOnReleaseBranch())
            {
                Checkout($"{ReleaseBranchPrefix}/{MajorMinorPatchVersion}", DevelopBranch);
                return;
            }

            FinishReleaseOrHotfix();
        });

    [PublicAPI]
    Target Hotfix => _ => _
        .DependsOn(Changelog)
        .Requires(() => !Repository.IsOnHotfixBranch() || GitHasCleanWorkingCopy())
        .Executes(() =>
        {
            var masterVersion = GitVersion(s => s
                .SetFramework("net6.0")
                .SetUrl(RootDirectory)
                .SetBranch(MasterBranch)
                .EnableNoFetch()
                .DisableProcessLogOutput()).Result;

            if (!Repository.IsOnHotfixBranch())
            {
                Checkout($"{HotfixBranch}/{masterVersion.Major}.{masterVersion.Minor}.{masterVersion.Patch + 1}",
                    MasterBranch);
                return;
            }

            FinishReleaseOrHotfix();
        });

    [PublicAPI]
    Target PublishGitHubRelease => _ => _
        .DependsOn(Pack)
        .OnlyWhenDynamic(() => Repository.IsOnReleaseBranch() || Repository.IsOnMainOrMasterBranch() ||
                               Repository.IsOnHotfixBranch())
        .Executes<Task>(async () =>
        {
            Log.Information("Started creating release");
            var releaseTag = $"v{GitVersion.MajorMinorPatch}";

            var changeLogSectionEntries = ExtractChangelogSectionNotes(From<IChangeLog>().ChangeLogFile);
            var latestChangeLog = changeLogSectionEntries
                .Aggregate((c, n) => c + System.Environment.NewLine + n);

            var completeChangeLog = $"## {releaseTag}" + System.Environment.NewLine + latestChangeLog;

            var (gitHubOwner, repositoryName) = GetGitHubRepositoryInfo(Repository);
            var nugetPackages = OutputDirectory.GlobFiles("*.nupkg").NotNull("Could not find nuget packages.")
                .Select(x => x.ToString()).ToArray();

            Log.Information($"Found total of {nugetPackages.Length} packages to publish.");

            await PublishRelease(conf => conf
                .SetArtifactPaths(nugetPackages)
                .SetCommitSha(GitVersion.Sha)
                .SetReleaseNotes(completeChangeLog)
                .SetRepositoryName(repositoryName)
                .SetRepositoryOwner(gitHubOwner)
                .SetTag(releaseTag)
                .DisablePrerelease()
            );
        });

    [PublicAPI]
    Target Push => _ => _
        .DependsOn(Pack)
        .OnlyWhenStatic(() => !string.IsNullOrEmpty(Settings.GitHubSettings.GithubSource))
        .Requires(() => NugetApiKey)
        .Executes(() =>
        {
            Log.Information("Running push to packages directory");
            GlobFiles(OutputDirectory, "*.nupkg").NotNull("Could not find nuget packages.")
                .Where(x => !x.EndsWith("symbols.nupkg"))
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSource(Settings.GitHubSettings.GithubSource)
                        .SetApiKey(NugetApiKey)
                    );
                });
        });


    [PublicAPI]
    Target Generate => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            GenerateCode("", _ => SourceDirectory / "Nuke.CoberturaConverter");
        });

    [PublicAPI]
    Target Coverage => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var testProjects = TestsDirectory.GlobFiles("*est*.csproj").ToList();
            if (testProjects.Count == 0)
            {
                throw new InvalidOperationException("Could not run test coverage since you have no tests defined.");
            }

            testProjects.ForEach((testProject, index) =>
            {
                var projectDirectory = Path.GetDirectoryName(testProject);
                var dotnetPath = ToolPathResolver.GetPathExecutable("dotnet");
                var snapshotIndex = index;

                var xUnitOutputDirectory = OutputDirectory / $"$test_{snapshotIndex:00}.testresults";
                DotCoverCover(c => c
                    .SetTargetExecutable(dotnetPath)
                    .SetTargetWorkingDirectory(projectDirectory)
                    .SetTargetArguments($"xunit -nobuild -xml {xUnitOutputDirectory.ToString().DoubleQuoteIfNeeded()}")
                    .SetFilters("+CoberturaConverter.Core")
                    .SetAttributeFilters("System.CodeDom.Compiler.GeneratedCodeAttribute")
                    .SetOutputFile(OutputDirectory / $"coverage{snapshotIndex:00}.snapshot")
                );
            });

            var snapshots = testProjects.Select((_, index) => OutputDirectory / $"coverage{index:00}.snapshot")
                .Select(p => p.ToString())
                .Aggregate((c, n) => c + ";" + n);

            DotCoverMerge(c => c
                .SetSource(snapshots)
                .SetOutputFile(OutputDirectory / "coverage.snapshot"));

            DotCoverReport(c => c
                .SetSource(OutputDirectory / "coverage.snapshot")
                .SetOutputFile(OutputDirectory / "coverage.xml")
                .SetReportType(DotCoverReportType.DetailedXml));


            // Jenkins report
            ReportGenerator(c => c
                .SetReports(OutputDirectory / "coverage.xml")
                .SetTargetDirectory(OutputDirectory / "CoverageReport"));
            return Task.CompletedTask;
        });

    void Checkout(string branch, string start)
    {
        var hasCleanWorkingCopy = GitHasCleanWorkingCopy();

        if (!hasCleanWorkingCopy && AutoStash)
        {
            Git($"stash");
        }

        Git($"checkout -b {branch} {start}");

        if (!hasCleanWorkingCopy && AutoStash)
        {
            Git($"stash apply");
        }
    }

    void FinishReleaseOrHotfix()
    {
        Git($"checkout {MasterBranch}");
        Git($"merge --no-ff --no-edit {Repository.Branch}");
        Git($"tag {MajorMinorPatchVersion}");

        Git($"checkout {DevelopBranch}");
        Git($"merge --no-ff --no-edit {Repository.Branch}");

        Git($"branch -D {Repository.Branch}");

        Git($"push origin {MasterBranch} {DevelopBranch} {MajorMinorPatchVersion}");
    }
}