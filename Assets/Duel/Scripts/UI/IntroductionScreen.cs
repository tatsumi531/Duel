using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionScreen : MonoBehaviour
{
    [SerializeField]Text firstText;
    [SerializeField]Text secondText;


    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        firstText.color = new Color(1f, 1f, 1f, 0f);
        firstText.transform.localScale = Vector3.one * 2f;
        secondText.color = new Color(1f, 1f, 1f, 0f);
        secondText.transform.localScale = Vector3.one * 3f;
    }

    public IEnumerator PlayFlow()
    {
        float a = 1f;
        float d = 0.5f;
        float d2 = 0.25f;
        Sequence seq = DOTween.Sequence();
        seq.Append(firstText.DOFade(a, d));
        seq.Join(firstText.transform.DOScale(a, d));
        seq.Append(firstText.DOFade(0f, d));
        seq.AppendInterval(d);
        seq.Append(secondText.DOFade(a, d2));
        seq.Join(secondText.transform.DOScale(a, d2));
        seq.AppendInterval(d);
        seq.AppendCallback(() => secondText.color = Color.clear);
        seq.Play();
        yield return new WaitWhile(seq.IsPlaying);
    }
}
