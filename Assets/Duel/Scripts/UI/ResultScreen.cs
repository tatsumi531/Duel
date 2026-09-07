using System;
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
        var elapsedTime = new TimeSpan((long)elapsedSeconds);
        elapsedTimeText.text = elapsedTime.ToString();
        canvasGroup.alpha = 1f;
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
