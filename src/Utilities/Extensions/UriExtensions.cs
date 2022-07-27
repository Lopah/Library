using System;
using JetBrains.Annotations;

namespace Utilities.Extensions;

[PublicAPI]
public static class UriExtensions
{
    public static string GetUsername(this Uri uri)
    {
        if (string.IsNullOrWhiteSpace(uri?.UserInfo))
        {
            return string.Empty;
        }

        var parts = uri.UserInfo.Split(':', 2);

        return parts.Length > 0 ? parts[0] : string.Empty;
    }

    public static string GetPassword(this Uri uri)
    {
        if (string.IsNullOrWhiteSpace(uri?.UserInfo))
        {
            return string.Empty;
        }

        var parts = uri.UserInfo.Split(':', 2);

        return parts.Length > 1 ? parts[1] : string.Empty;
    }
}