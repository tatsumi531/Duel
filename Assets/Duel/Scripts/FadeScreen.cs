using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] float duration = 0.2f;

    void Awake()
    {
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = true;
    }

    public void SetColor(Color color)
    {
        fadeImage.color = color;
    }

    public async Awaitable FadeOutAsync(CancellationToken token)
    {
        fadeImage.raycastTarget = true;
        await FadeAsync(1f, token);
    }

    public async Awaitable FadeInAsync(CancellationToken token)
    {
        await FadeAsync(0f, token);
        fadeImage.raycastTarget = false;
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
