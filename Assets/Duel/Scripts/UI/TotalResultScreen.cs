using System;
using UnityEngine;
using UnityEngine.UI;

public class TotalResultScreen : ResultScreen
{
    [SerializeField]Text result;
    [SerializeField]Text totalElapsed;
    [SerializeField]Button returnButton;
    public event Action OnClicked;


    void Start()
    {
        returnButton.onClick.AddListener(() => OnClicked?.Invoke());
        Hide();
    }

    protected override void SetContent(ShowType showAs, GameContext gameContext)
    {
        int winRounds = gameContext.RoundCounter.WinRoundCount;
        string displayText = "";
        switch (showAs) {
            case ShowType.Win:
                displayText = winRounds.ToString() + "人抜き達成!!";
            break;
            case ShowType.Lose:
                displayText = winRounds > 0 ? winRounds.ToString() + "人抜き" : "敗北";
            break;
            default:
                Debug.LogError("Error ShowType");
            break;
        }
        result.text = displayText;
        totalElapsed.text = FormatElapsedTime(gameContext.RoundTimer.TotalElapsed);
    }
}
