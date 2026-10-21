using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [SerializeField] TitleScreen title;
    [SerializeField] IntroductionScreen introduction;
    [SerializeField] RoundResultScreen roundResult;
    [SerializeField] TotalResultScreen totalResult;
    [SerializeField] FadeScreen fade;

    public TitleScreen Title => title;
    public IntroductionScreen Introduction => introduction;
    public RoundResultScreen RoundResult => roundResult;
    public TotalResultScreen TotalResult => totalResult;
    public FadeScreen Fade => fade;
}
