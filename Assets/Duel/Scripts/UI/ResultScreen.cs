using UnityEngine;

public abstract class ResultScreen : MonoBehaviour
{
    public enum ShowType
    {
        Win,
        Lose
    }

    [SerializeField]CanvasGroup canvasGroup;


    protected abstract void SetContent(ShowType showAs, GameContext gameContext);

    public void Show(ShowType showAs, GameContext gameContext)
    {
        SetContent(showAs, gameContext);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    protected string FormatElapsedTime(float elapsedSeconds)
    {
        // 分と秒の境の四捨五入のズレを回避するために1/100秒単位で分解する。
        long totalCentiseconds = (long)Mathf.Round(elapsedSeconds * 100f);
        long minutes = totalCentiseconds / 6000;
        float seconds = totalCentiseconds % 6000 / 100f;

        return minutes > 0 ? $"{minutes}:{seconds:00.00}" + "秒" : $"{seconds:0.00}" + "秒";
    }
}
