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

    protected override void SetContent(ShowType showAs, GameContext gameContext)
    {
        switch (showAs) {
            case ShowType.Win: resultText.text = "勝利"; break;
            case ShowType.Lose: resultText.text = "敗北"; break;
        }
        elapsedTimeText.text = FormatElapsedTime(gameContext.RoundTimer.RoundElapsed);
    }
}
