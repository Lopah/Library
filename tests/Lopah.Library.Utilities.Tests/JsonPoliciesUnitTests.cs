using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Utilities.Policies;
using Xunit;

namespace Lopah.Library.Utilities.Tests;

public class JsonPolicies
{
    private enum Test
    {
        HelloWorld
    }
    private class Data
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Test Something { get; set; }
        
    }
    [Fact]
    public void SnakeCaseNamingPolicy_GivenTwoWordEnum_SnakeCasesIt()
    {
        var test = new Data
        {
            Something = Test.HelloWorld
        };

        var data = JsonSerializer.Serialize(test, new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicies.SnakeCase) }
        });

        data.Should().NotBeEmpty();
        data.Should().Match(s => s.Contains("hello_world", StringComparison.InvariantCulture));
    }
    
    [Fact]
    public void UpperSnakeCaseNamingPolicy_GivenTwoWordEnum_UpperSnakeCasesIt()
    {
        var test = new Data
        {
            Something = Test.HelloWorld
        };

        var data = JsonSerializer.Serialize(test, new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicies.UpperSnakeCase) }
        });

        data.Should().NotBeEmpty();
        data.Should().Match(s => s.Contains("HELLO_WORLD", StringComparison.InvariantCulture));
    }
}