using System;

namespace Api;

public class CorrelationOptions
{
    public bool UseSerilog { get; set; } = false;
    
    public string HeaderKey { get; set; } = "CorrelationId";
    
    public bool ReplaceTraceIdentifier { get; set; } = true;
    
    public bool AddResponseHeader { get; set; } = true;
    
    public Func<string> SetCorrelationId { get; set; } = () => Guid.NewGuid().ToString("D");
}