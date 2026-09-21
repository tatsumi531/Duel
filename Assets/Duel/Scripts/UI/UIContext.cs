using UnityEngine;

public class UIContext
{
    public TitleScreen Title { get; }
    public IntroductionScreen Introduction { get; }
    public RoundResultScreen RoundResult { get; }
    public TotalResultScreen TotalResult { get; }


    public UIContext()
    {
        Title = GameObject.FindAnyObjectByType<TitleScreen>();
        Introduction = GameObject.FindAnyObjectByType<IntroductionScreen>();
        RoundResult = GameObject.FindAnyObjectByType<RoundResultScreen>();
        TotalResult = GameObject.FindAnyObjectByType<TotalResultScreen>();
    }
}
