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
        if (gameContext.RoundCounter.CurrentRound == 1) {
            result.text = "敗北";
        } else {
            result.text = (gameContext.RoundCounter.CurrentRound - 1).ToString() + "人抜き";
        }

        totalElapsed.text = FormatElapsedTime(gameContext.RoundTimer.TotalElapsed);
    }
}
