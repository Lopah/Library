using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Utilities.Converters;
using Utilities.Policies;

namespace Api.Extensions;

[PublicAPI]
public static class JsonExtensions
{
    public static void AddApiJsonOptions(this JsonOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        SetupJsonOptions(options.JsonSerializerOptions);
    }

    public static void AddApiJsonOptions(this JsonHubProtocolOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        SetupJsonOptions(options.PayloadSerializerOptions);
    }

    private static void SetupJsonOptions(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new DateTimeConverter());
        options.Converters.Add(new DateOnlyConverter());
        options.Converters.Add(new DateConverter());
        options.Converters.Add(new TrimmingConverter());
        options.Converters.Add(new TimeSpanConverter());
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicies.UpperSnakeCase));
    }
}