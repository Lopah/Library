using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Lopah.Library.Utilities.Tests.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void SplitList_WithNullList_ThrowArgumentNullException()
    {
        List<string>? list = null;

        var split = () => list!.SplitList().ToList();

        split.Should().ThrowExactly<ArgumentNullException>().WithParameterName("locations");
    }


    [Fact]
    public void SplitList_WithListOfTenItemsAndChunkOf2_Returns2ListOf5Elements()
    {
        var list = new List<string>
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"
        };

        var split = list.SplitList(5).ToList();


        split.Should().HaveCount(2);

        split.First().Should().HaveCount(5);
        split.Last().Should().HaveCount(5);
    }


    [Fact]
    public void MaxOrDefault_WithNullListAndSelector_ReturnsDefault()
    {
        List<int> nullList = null!;

        var result = nullList.MaxOrDefault(e => e);

        result.Should().Be(0);
    }

    [Fact]
    public void MaxOrDefault_WithNullList_ReturnsDefault()
    {
        List<int> nullList = null!;

        var result = nullList.MaxOrDefault();

        result.Should().Be(0);
    }


    [Fact]
    public void MaxOrDefault_WithEmptyListAndSelector_ReturnsDefault()
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        var emptyList = new List<int>();

        var result = emptyList.MaxOrDefault(e => e);

        result.Should().Be(0);
    }

    [Fact]
    public void MaxOrDefault_WithEmptyList_ReturnsDefault()
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        var emptyList = new List<int>();

        var result = emptyList.MaxOrDefault();

        result.Should().Be(0);
    }

    [Fact]
    public void MaxOrDefault_WithListOfIntegersAndSelector_ReturnsLargest()
    {
        var list = new List<int>
        {
            1,
            2,
            3,
            4,
            5
        };

        var result = list.MaxOrDefault(e => e);

        result.Should().Be(5);
    }

    [Fact]
    public void MaxOrDefault_WithListOfIntegers_ReturnsLargest()
    {
        var list = new List<int>
        {
            1,
            2,
            3,
            4,
            5
        };

        var result = list.MaxOrDefault();

        result.Should().Be(5);
    }


    [Fact]
    public void HasEqualItems_WithEmptySource_ThrowsArgumentNullException()
    {
        List<int>? nullList = null;

        var someList = new List<int>
        {
            1,
            2,
            3
        };

        var result = () => nullList!.HasEqualItems(someList, (first, second) => first.Equals(second));

        result.Should().ThrowExactly<ArgumentNullException>().WithParameterName("source");
    }

    [Fact]
    public void HasEqualItems_WithEmptyDestination_ThrowsArgumentNullException()
    {
        List<int>? nullList = null;

        var someList = new List<int>
        {
            1,
            2,
            3
        };

        var result = () => someList.HasEqualItems(nullList!, (first, second) => first.Equals(second));

        result.Should().ThrowExactly<ArgumentNullException>().WithParameterName("destination");
    }

    [Fact]
    public void HasEqualItems_WithOneListLarger_ReturnsFalse()
    {
        var someList = new List<int>
        {
            1,
            2,
            3
        };

        var someOtherList = new List<int>
        {
            1,
            2,
            3,
            4
        };

        var result = someList.HasEqualItems(someOtherList, (first, second) => first.Equals(second));

        result.Should().BeFalse();
    }

    [Fact]
    public void HasEqualItems_WithNonEqualLastElement_ReturnsFalse()
    {
        var someList = new List<int>
        {
            1,
            2,
            3
        };

        var someOtherList = new List<int>
        {
            1,
            2,
            5
        };

        var result = someList.HasEqualItems(someOtherList, (first, second) => first.Equals(second));

        result.Should().BeFalse();
    }

    [Fact]
    public void HasEqualItems_WithEqualLists_ReturnsTrue()
    {
        var someList = new List<int>
        {
            1,
            2,
            3
        };

        var someOtherList = new List<int>
        {
            1,
            2,
            3
        };

        var result = someList.HasEqualItems(someOtherList, (first, second) => first.Equals(second));

        result.Should().BeTrue();
    }

    [Fact]
    public void HasAny_WithNullSource_ReturnsFalse()
    {
        List<string>? nullList = null;

        var result = nullList.HasAny();

        result.Should().BeFalse();
    }

    [Fact]
    public void HasAny_WithNullSourceAndNegated_ReturnsTrue()
    {
        List<string>? nullList = null;

        var result = !nullList.HasAny();

        result.Should().BeTrue();
    }

    [Fact]
    public void HasAny_WithSomeItems_ReturnsFalse()
    {
        var nullList = new List<string>
        {
            "something"
        };

        var result = nullList.HasAny();

        result.Should().BeTrue();
    }

    [Fact]
    public void AddIfAnyAsync_WithNullSource_ThrowsArgumentNullException()
    {
        List<string>? nullList = null;
        var someCollection = new List<string>
        {
            "something",
            "else"
        };

        var cnclTokenSource = new CancellationTokenSource();

        var cancellationToken = cnclTokenSource.Token;

        var result = () => nullList!.AddIfAnyAsync(
            someCollection,
            (enumerable, token) => { return Task.FromResult(enumerable.Single(e => e.Contains("something"))).WithCancellation(token); },
            cancellationToken);

        result.Should().ThrowExactlyAsync<ArgumentNullException>()
            .WithParameterName("source");
    }

    [Fact]
    public void AddIfAnyAsync_WithNullDestination_ThrowsArgumentNullException()
    {
        List<string>? nullList = null;
        var someCollection = new List<string>
        {
            "something",
            "else"
        };

        var cnclTokenSource = new CancellationTokenSource();

        var cancellationToken = cnclTokenSource.Token;

        var result = () => nullList!.AddIfAnyAsync(
            someCollection,
            (_, token) => Task.FromResult<string>(null!).WithCancellation(token),
            cancellationToken);

        result.Should().ThrowExactlyAsync<ArgumentNullException>()
            .WithParameterName("onAdd");
    }

    [Fact]
    public async Task AddIfAnyAsync_WithProperCondition_AddsCorrectly()
    {
        var initialList = new List<string>
        {
            "easy",
            "peasy"
        };
        var someCollection = new List<string>
        {
            "something",
            "else"
        };

        var cnclTokenSource = new CancellationTokenSource();

        var cancellationToken = cnclTokenSource.Token;

        await initialList.AddIfAnyAsync(
            someCollection,
            (enumerable, token) => { return Task.FromResult(enumerable.Single(e => e.Contains("something"))).WithCancellation(token); },
            cancellationToken);

        var expectedCollection = new List<string>
        {
            "easy",
            "peasy",
            "something"
        };
        initialList.Count.Should().Be(3);
        initialList.Should().BeEquivalentTo(expectedCollection);
    }

    [Fact]
    public void ForEach_CountWithEachIteration_ReturnsCorrectNumberOfIterations()
    {
        var count = 0;
        var list = new ReadOnlyCollection<string>(
            new List<string>
            {
                "something",
                "crazy",
                "is",
                "here"
            });

        list.ForEach(_ => count += 1);

        count.Should().Be(list.Count);
    }

    [Fact]
    public void ForEach_WithNullSource_ThrowsArgumentNullException()
    {
        var count = 0;
        ReadOnlyCollection<string> list = null!;

        var func = () => list!.ForEach(_ => count += 1);

        func.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("source");
        count.Should().Be(0);
    }

    [Fact]
    public void ForEach_WithNullAction_ThrowsArgumentNullException()
    {
        var list = new ReadOnlyCollection<string>(
            new List<string>
            {
                "something",
                "crazy",
                "is",
                "here"
            });

        var func = () => list.ForEach(null!);

        func.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("action");
    }

    [Fact]
    public async Task AddRangeIfAny_WithNullSource_ThrowsArgumentNullException()
    {
        var tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;
        var list = new List<int>
        {
            1, 2, 3
        };

        var toAdd = new List<int>
        {
            4, 5, 6
        };

        await list.AddRangeIfAnyAsync(
            toAdd,
            (integers, _) => Task.FromResult(integers.Where(e => e % 2 == 0)),
            cancellationToken);


        var output = new List<int>
        {
            1, 2, 3, 4, 6
        };

        list.Should().BeEquivalentTo(output);
    }

    [Fact]
    public async Task AddRangeIfAny_WithAddListOfLists_AddsFirstOne()
    {
        var tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;
        var list = new List<int>
        {
            1, 2, 3
        };

        var toAdd = new List<List<int>>
        {
            new List<int>
            {
                1, 2, 3
            },
            new List<int>
            {
                1, 1, 1, 1
            }
        };

        await list.AddRangeIfAnyAsync(
            toAdd,
            (something, _) => { return Task.FromResult(something.First(e => e.Count == 3).AsEnumerable()); },
            cancellationToken);

        var output = new List<int>
        {
            1, 2, 3, 1, 2, 3
        };

        list.Should().BeEquivalentTo(output);
    }
}