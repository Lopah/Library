using JetBrains.Annotations;

namespace Nuke.Build.Custom.GitHub;

[PublicAPI]
public class Branch
{
    public const string MasterBranch = "master";
    public const string MainBranch = "main";
    public const string DevelopBranch = "develop";
    public const string ReleaseBranchPrefix = "release";
    public const string HotfixBranch = "hotfix";
    public static bool IsMainOrMaster(string input) => input.Contains(MasterBranch) || input.Contains(MainBranch);
}