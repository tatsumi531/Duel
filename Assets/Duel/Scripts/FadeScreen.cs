using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance { get; private set; }
    public bool IsFading { get { return fadeAction != null;} }

    [SerializeField] Image fadeImage;
    [SerializeField] float duration = 0.2f;
    Awaitable fadeAction = null;

    void Awake()
    {
        Instance = this;
    }

    public void StartFade(Color to)
    {
        FadeFlowAsync(fadeImage.color, to);
    }

    public void StartFade(Color from, Color to)
    {
        FadeFlowAsync(from, to);
    }

    async void FadeFlowAsync(Color from, Color to)
    {
        fadeImage.raycastTarget = true;
        fadeAction?.Cancel();
        fadeAction = FadeAsync(from, to);
        try
        {
            await fadeAction;
        }
        catch (OperationCanceledException)
        {
            return;
        }
        fadeImage.raycastTarget = false;
        fadeAction = null;
    }

    async Awaitable FadeAsync(Color from, Color to)
    {
        float elapsed = 0f;
        fadeImage.color = from;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = Color.Lerp(from, to, elapsed / duration);
            await Awaitable.NextFrameAsync(destroyCancellationToken);
        }
        fadeImage.color = to;
    }
}
