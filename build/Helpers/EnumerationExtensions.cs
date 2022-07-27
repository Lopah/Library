using JetBrains.Annotations;
using Nuke.Common.Tooling;

namespace Nuke.Build.Custom.Helpers;

[UsedImplicitly]
public static class EnumerationExtensions
{
    public static string ValueToLower(this Enumeration enumeration) => enumeration.ToString().ToLower();
}