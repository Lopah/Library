using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Validation;

public sealed class EmailAddressValidationAttribute : ValidationAttribute, IPatternValidationAttribute
{
    // RFC 5322 - source: https://www.regular-expressions.info/email.html
    public const string Pattern =
        @"\A(?=[a-zA-Z0-9@.!#$%&'*+/=?^_`{|}~-]{6,254}\z)(?=[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]{1,64}@)[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:(?=[a-zA-Z0-9-]{1,63}\.)[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+(?=[a-zA-Z0-9-]{1,63}\z)[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\z";

    private static readonly Regex _regex = Pattern.GetCompiledRegex();

    string IPatternValidationAttribute.Pattern => Pattern;

    /// <summary>Determines whether the specified value of the object is valid.</summary>
    /// <param name="value">The value of the object to validate.</param>
    /// <exception cref="T:System.InvalidOperationException">The current attribute is malformed.</exception>
    /// <exception cref="T:System.NotImplementedException">
    ///     Neither overload of <see langword="IsValid" /> has been implemented
    ///     by a derived class.
    /// </exception>
    /// <returns>
    ///     <see langword="true" /> if the specified value is valid; otherwise, <see langword="false" />.
    /// </returns>
    public override bool IsValid(object? value)
    {
        return _regex.IsValid(value);
    }
}