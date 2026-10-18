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
        // 分と秒の境の四捨五入のズレを回避するために1/1000秒単位で分解する。
        long totalMilliseconds = (long)Mathf.Round(elapsedSeconds * 1000f);
        long minutes = totalMilliseconds / 60000;
        float seconds = totalMilliseconds % 60000 / 1000f;

        return minutes > 0 ? $"{minutes}:{seconds:00.000}" + "秒" : $"{seconds:0.000}" + "秒";
    }
}
