﻿using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Utilities.Extensions;

[PublicAPI]
public static class TaskExtensions
{
    public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        var tcs = new TaskCompletionSource<object>();
        await using (cancellationToken.Register(tcs.SetCanceled))
        {
            var finishedTask = await Task.WhenAny(task, tcs.Task);

            if (finishedTask == tcs.Task)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            await task;
        }
    }

    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        var tcs = new TaskCompletionSource<object>();
        await using (cancellationToken.Register(tcs.SetCanceled))
        {
            var finishedTask = await Task.WhenAny(task, tcs.Task);

            if (finishedTask == tcs.Task)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            return await task;
        }
    }
}