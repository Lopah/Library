using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class DefaultApiVersionOptions
{
    [Required]
    public int? Major { get; init; }

    [Required]
    public int? Minor { get; init; }
}