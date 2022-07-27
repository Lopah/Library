using Nuke.Common;

namespace Nuke.Build.Custom.Components;

public interface IChangeLog : INukeBuild
{
    string ChangeLogFile => RootDirectory / "CHANGELOG.md";
}