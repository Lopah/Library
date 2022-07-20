using System.ComponentModel.DataAnnotations;
using Validation;

namespace OpenApi.Options;

public class ContactOptions
{
    [Required]
    [EmailAddressValidation]
    public string Email { get; init; } = null!;

    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public Uri Url { get; init; } = null!;
}