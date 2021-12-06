using System.ComponentModel.DataAnnotations;
using Validation;

namespace OpenApi.Options;

public class ContactOptions
{
    [Required] [EmailAddressValidation] public string Email { get; init; }

    [Required] public string Name { get; init; }

    [Required] public Uri Url { get; init; }
}
