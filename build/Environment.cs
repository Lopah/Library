using System.ComponentModel;
using Nuke.Common.Tooling;

namespace Nuke.Build.Custom;

[TypeConverter(typeof(TypeConverter<Environment>))]
public class Environment : Enumeration
{
    public static readonly Environment Undefined = new() { Value = nameof(Undefined) };
    public static readonly Environment Development = new() { Value = nameof(Development) };
    public static readonly Environment Test = new() { Value = nameof(Test) };
    public static readonly Environment Production = new() { Value = nameof(Production) };

    public static bool EnvironmentIs(Environment environment) => ((object)Build.Environment).Equals(environment);

    public static implicit operator string(Environment environment) => environment.Value;
}