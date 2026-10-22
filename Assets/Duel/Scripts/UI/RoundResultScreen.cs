using UnityEngine;
using UnityEngine.UI;

public class RoundResultScreen : ResultScreen
{
    [SerializeField] Text resultText;
    [SerializeField] Text elapsedTimeText;


    void Start()
    {
        Hide();
    }

    public void Show(bool isWin, float roundElapsedSeconds)
    {
        resultText.text = isWin ? "勝利" : "敗北";
        elapsedTimeText.text = ElapsedTimeFormatter.Format(roundElapsedSeconds);
        ShowCanvas();
    }
}
