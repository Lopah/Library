using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Validation;
using Xunit;

namespace Lopah.Library.Validation.Tests;

public class ValidationUnitTests
{
    [Fact]
    public void Test()
    {
        var data = new Data
        {
            Email = "jasekdan@gmail.com"
        };

        var ctx = new ValidationContext(data);

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(data, ctx, results, true);

        isValid.Should().BeTrue();

        results.Should().BeEmpty();
    }

    [Fact]
    public void EmailValidation_GivenFkedUpGeneratedEmail_ShouldStillBeTrue()
    {
        var data = new Data
        {
            Email = "jasekdan+someDomainThatShouldStillBeValid@some.domain.com"
        };

        var ctx = new ValidationContext(data);

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(data, ctx, results, true);

        isValid.Should().BeTrue();

        results.Should().BeEmpty();
    }

    private class Data
    {
        [EmailAddressValidation]
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string Email { get; set; } = string.Empty;
    }
}