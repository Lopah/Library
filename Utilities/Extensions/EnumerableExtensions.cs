namespace Utilities.Extensions;

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

    public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source)
    {
        if (source.IsAny())
        {
            return source.Max();
        }

        return default!;
    }

    public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TResult> selector)
    {
        if (source.IsAny())
        {
            return source.Max(selector);
        }

        return default!;
    }

    public static bool HasEqualItems<T>(this IReadOnlyCollection<T> source, IReadOnlyCollection<T> destination,
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

    public static bool IsAny<T>(this IEnumerable<T> source)
    {
        return source?.Any() == true;
    }

    public static async Task AddIfAny<TSource, T>(this List<TSource> source, IEnumerable<T> collection,
        Func<IEnumerable<T>, CancellationToken, Task<TSource>> onAdd, CancellationToken cancellationToken)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (onAdd is null)
        {
            throw new ArgumentNullException(nameof(onAdd));
        }

        if (collection.IsAny())
        {
            source.Add(await onAdd(collection, cancellationToken));
        }
    }

    public static async Task AddRangeIfAny<TSource, T>(this List<TSource> source, IEnumerable<T> collection,
        Func<IEnumerable<T>, CancellationToken, Task<IEnumerable<TSource>>> onAdd, CancellationToken cancellationToken)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (onAdd is null)
        {
            throw new ArgumentNullException(nameof(onAdd));
        }

        if (collection.IsAny())
        {
            source.AddRange(await onAdd(collection, cancellationToken));
        }
    }

    public static async Task AddRangeIfAny<TSource, T>(this List<TSource> source, IEnumerable<T> collection,
        Func<IEnumerable<T>, CancellationToken, Task<IReadOnlyCollection<TSource>>> onAdd,
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

        if (collection.IsAny())
        {
            source.AddRange(await onAdd(collection, cancellationToken));
        }
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
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

    public static void ForEach<T>(this IReadOnlyList<T> source, Action<T, int> action)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        for (var i = 0; i < source.Count; ++i)
        {
            action(source[i], i);
        }
    }
}