using System;
using System.Threading;
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
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = true;
    }

    public void SetColor(Color color)
    {
        fadeImage.color = color;
    }

    public void StartFadeOut(CancellationToken token)
    {
        FadeOutAsync(token);
    }

    public void StartFadeIn(CancellationToken token)
    {
        FadeInAsync(token);
    }

    async void FadeOutAsync(CancellationToken token)
    {
        fadeAction?.Cancel();
        fadeImage.raycastTarget = true;
        fadeAction = FadeAsync(1f, token);
        try
        {
            await fadeAction;
        }
        catch (OperationCanceledException)
        {
            return;
        }
        fadeAction = null;
    }

    async void FadeInAsync(CancellationToken token)
    {
        fadeAction?.Cancel();
        fadeAction = FadeAsync(0f, token);
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

    async Awaitable FadeAsync(float alphaTo, CancellationToken token)
    {
        float elapsed = 0f;
        float alphaFrom = fadeImage.color.a;
        Color color = fadeImage.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(alphaFrom, alphaTo, elapsed / duration);
            fadeImage.color = color;
            await Awaitable.NextFrameAsync(token);
        }
        color.a = alphaTo;
        fadeImage.color = color;
    }
}
