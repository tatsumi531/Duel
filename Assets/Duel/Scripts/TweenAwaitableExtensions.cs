using System;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public static class TweenAwaitableExtensions
{
    public static async Awaitable WaitForCompletionAsync(this Tween tween, CancellationToken token)
    {
        try {
            while (tween.IsActive() && !tween.IsComplete()) {
                await Awaitable.NextFrameAsync(token);
            }
        }
        catch (OperationCanceledException) {
            tween.Kill();
            throw;
        }
    }
}
