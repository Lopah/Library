using System.Threading.Tasks;
using EntityFrameworkCore.Tests.Base;
using Xunit;

namespace EntityFrameworkCore.Tests;

[Collection("ConditionalWhere")]
public class ConditionalWhereTests
{
    private readonly ApplicationDbContext _applicationDbContext;
    public ConditionalWhereTests()
    {
        _applicationDbContext = ApplicationFake.DbContext;
    }
    
    [Fact]
    public Task PureConditionalWhere_GivenFalsyCondition_ReturnsTheSameQueryableSource()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public Task PureConditionalWhere_GivenTruthyCondition_ReturnsAppliedPredicateOnQueryableSource()
    {
        return Task.CompletedTask;
    }
}
