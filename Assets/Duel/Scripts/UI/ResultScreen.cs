using UnityEngine;

public abstract class ResultScreen : MonoBehaviour
{
    [SerializeField]CanvasGroup canvasGroup;


    protected void ShowCanvas()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }
}
