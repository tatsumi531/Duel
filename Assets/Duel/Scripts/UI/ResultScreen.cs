using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] Text resultText;
    [SerializeField] Text elapsedTimeText;
    [SerializeField] CanvasGroup canvasGroup;


    void Start()
    {
        Hide();
    }

    public void Show(ShowType showAs, float elapsedSeconds)
    {
        switch (showAs) {
            case ShowType.Win: resultText.text = "勝利"; break;
            case ShowType.Lose: resultText.text = "敗北"; break;
        }
        elapsedTimeText.text = FormatElapsedTime(elapsedSeconds);
        canvasGroup.alpha = 1f;
    }

    static string FormatElapsedTime(float elapsedSeconds)
    {
        // 1/100秒単位で分解すると分と秒の境の四捨五入ズレを回避できる。
        long totalCentiseconds = (long)Mathf.Round(elapsedSeconds * 100f);
        long minutes = totalCentiseconds / 6000;
        float seconds = totalCentiseconds % 6000 / 100f;

        return minutes > 0 ? $"{minutes}:{seconds:00.00}" + "秒" : $"{seconds:0.00}" + "秒";
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }

    public enum ShowType
    {
        Win,
        Lose
    }
}
