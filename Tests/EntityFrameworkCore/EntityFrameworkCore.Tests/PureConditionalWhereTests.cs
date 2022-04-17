using System.Threading.Tasks;
using EntityFrameworkCore.Tests.Base;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EntityFrameworkCore.Tests;

[Collection("PureConditionalWhere")]
public class PureConditionalWhereTests
{
    private readonly ApplicationDbContext _applicationDbContext;

    public PureConditionalWhereTests()
    {
        _applicationDbContext = ApplicationFake.DbContext;
    }
    
    [Fact]
    public async Task PureConditionalWhere_GivenFalsyCondition_ReturnsAppliedPredicateOnQueryableSource()
    {
        var falsyCondition = false;
        var test = await _applicationDbContext.Cars.PureConditionalWhere(_ => falsyCondition, e => e.Bought == true)
            .ToListAsync();

        test.Count.Should().Be(5);
    }

    [Fact]
    public async Task PureConditionalWhere_GivenTruthyCondition_ReturnsAppliedPredicateOnQueryableSource()
    {
        var truthyCondition = true;
        var test = await _applicationDbContext.Cars.PureConditionalWhere(_ => truthyCondition, e => e.Bought == true)
            .ToListAsync();

        test.Count.Should().Be(3);
    }
}
