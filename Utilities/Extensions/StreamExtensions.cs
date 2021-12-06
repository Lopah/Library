namespace Utilities.Extensions;

public static class StreamExtensions
{
    public static async Task CopyTo(this Stream destination, Stream source, CancellationToken cancellationToken)
    {
        if (destination is null) throw new ArgumentNullException(nameof(destination));

        if (source is null) throw new ArgumentNullException(nameof(source));

        await source.CopyToAsync(destination, cancellationToken);
        destination!.Position = 0;
    }
}
