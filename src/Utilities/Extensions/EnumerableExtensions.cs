using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Utilities.Extensions;

[PublicAPI]
public static class EnumerableExtensions
{
    public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int chunkSize = 30)
    {
        if (locations is null)
        {
            throw new ArgumentNullException(nameof(locations));
        }

        for (var i = 0; i < locations.Count; i += chunkSize)
        {
            yield return locations.GetRange(i, Math.Min(chunkSize, locations.Count - i));
        }
    }

    public static TSource? MaxOrDefault<TSource>(this IEnumerable<TSource>? source)
    {
        if (source is null)
        {
            return default;
        }

        var enumerable = source.ToList();
        return enumerable.HasAny() ? enumerable.Max() : default;
    }

    public static TResult? MaxOrDefault<TSource, TResult>(
        this IEnumerable<TSource>? source,
        Func<TSource, TResult> selector)
    {
        if (source is null)
        {
            return default;
        }

        var enumerable = source.ToList();
        return enumerable.HasAny() ? enumerable.Max(selector) : default;
    }

    public static bool HasEqualItems<T>(
        this IReadOnlyCollection<T> source,
        IReadOnlyCollection<T> destination,
        Func<T, T, bool> predicate)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (destination is null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (source.Count != destination.Count)
        {
            return false;
        }

        foreach (var sourceItem in source)
        {
            if (destination.Count(destinationItem => predicate(sourceItem, destinationItem)) != 1)
            {
                return false;
            }
        }

        return true;
    }

    public static bool HasAny<T>(this IEnumerable<T>? source)
    {
        return source?.Any() == true;
    }

    public static async Task AddIfAnyAsync<TSource, T>(
        this List<TSource> source,
        IEnumerable<T>? collection,
        Func<IEnumerable<T>, CancellationToken, Task<TSource>> onAdd,
        CancellationToken cancellationToken)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (onAdd is null)
        {
            throw new ArgumentNullException(nameof(onAdd));
        }

        var enumerable = collection?.ToList();
        if (enumerable.HasAny())
        {
            source.Add(await onAdd(enumerable!, cancellationToken));
        }
    }

    public static async Task AddRangeIfAnyAsync<TSource, T>(
        this List<TSource> source,
        IEnumerable<T> collection,
        Func<IEnumerable<T>, CancellationToken, Task<IEnumerable<TSource>>> onAdd,
        CancellationToken cancellationToken)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (onAdd is null)
        {
            throw new ArgumentNullException(nameof(onAdd));
        }

        var enumerable = collection.ToList();
        if (enumerable.HasAny())
        {
            source.AddRange(await onAdd(enumerable, cancellationToken));
        }
    }

    /// <summary>
    ///     Works same as <see cref="List{T}.ForEach" />
    /// </summary>
    /// <param name="source">source collection</param>
    /// <param name="action">what action to do on each element</param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ArgumentNullException">If the source is null</exception>
    /// <exception cref="ArgumentNullException">If the action is null</exception>
    public static void ForEach<T>(this IReadOnlyCollection<T> source, Action<T> action)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        foreach (var o in source)
        {
            action(o);
        }
    }
}