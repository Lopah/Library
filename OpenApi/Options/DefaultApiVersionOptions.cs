using System.ComponentModel.DataAnnotations;

namespace OpenApi.Options;

public class DefaultApiVersionOptions
{
    [Required] public int? Major { get; init; }

    [Required] public int? Minor { get; init; }
}
