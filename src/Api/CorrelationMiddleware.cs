using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Api;

[PublicAPI]
public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CorrelationOptions _options;

    public CorrelationMiddleware(RequestDelegate next, Func<CorrelationOptions, CorrelationOptions>? optionsAction = null)
    {
        var options = new CorrelationOptions();
        optionsAction?.Invoke(options);
        _options = options;
        _next = next;
    }
    
    public Task Invoke(HttpContext context)
    {
        var correlationId = GetCorrelationId(context);

        if (_options.AddResponseHeader)
        {
            context.Response.Headers.TryAdd(_options.HeaderKey, correlationId);
        }

        if (_options.ReplaceTraceIdentifier)
        {
            context.TraceIdentifier = correlationId;
        }

        if (!_options.UseSerilog)
        {
            return _next.Invoke(context);
        }

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            return _next.Invoke(context);
        }

    }
    
    private string GetCorrelationId(HttpContext context)
    {
        context.Request.Headers.TryGetValue(_options.HeaderKey, out var correlationId);
        
        return correlationId.FirstOrDefault() ?? _options.SetCorrelationId();
    }
}