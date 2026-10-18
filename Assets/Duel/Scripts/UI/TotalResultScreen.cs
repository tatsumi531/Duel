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
        string displayText;
        if (gameContext.RoundCounter.CurrentRound == 1) {
            displayText = "敗北";
        } else {
            displayText = (gameContext.RoundCounter.CurrentRound - 1).ToString() + "人抜き";
            if (!gameContext.RoundCounter.ExistNextRound(gameContext.GameData)) {
                displayText += "達成!!";
            }
        }
        result.text = displayText;
        totalElapsed.text = FormatElapsedTime(gameContext.RoundTimer.TotalElapsed);
    }
}
