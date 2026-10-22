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

    public void Show(bool isAllWin, int winRoundCount, float totalElapsedSeconds)
    {
        result.text = TotalResultText.Build(isAllWin, winRoundCount);
        totalElapsed.text = ElapsedTimeFormatter.Format(totalElapsedSeconds);
        ShowCanvas();
    }
}
