using System.ComponentModel;
using Nuke.Common.Tooling;

namespace Nuke.Build.Custom;

[TypeConverter(typeof(TypeConverter<Environment>))]
public class Environment : Enumeration
{
    public static readonly Environment Undefined = new Environment { Value = nameof(Undefined) };
    public static readonly Environment Development = new Environment { Value = nameof(Development) };
    public static readonly Environment Test = new Environment { Value = nameof(Test) };
    public static readonly Environment Production = new Environment { Value = nameof(Production) };

    public static bool EnvironmentIs(Environment environment) => ((object)Build.Environment).Equals(environment);

    public static implicit operator string(Environment environment) => environment.Value;
}