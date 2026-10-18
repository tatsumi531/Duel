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
        return ElapsedTimeFormatter.Format(elapsedSeconds);
    }
}
