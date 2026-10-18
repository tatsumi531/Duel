using NUnit.Framework;

public class RoundTimerTests
{
    RoundTimer timer;

    [SetUp]
    public void SetUp()
    {
        timer = new RoundTimer();
    }

    [Test]
    public void ResetRoundでラウンド経過時間のみリセットされる()
    {
        timer.OnUpdate(5f);
        timer.ResetRound();
        Assert.That(timer.RoundElapsed, Is.EqualTo(0f).Within(0.0001f));
        Assert.That(timer.TotalElapsed, Is.EqualTo(5f).Within(0.0001f));
    }

    [Test]
    public void ResetTotalで合計経過時間のみがリセットされる()
    {
        timer.OnUpdate(5f);
        timer.ResetTotal();
        Assert.That(timer.RoundElapsed, Is.EqualTo(5f).Within(0.0001f));
        Assert.That(timer.TotalElapsed, Is.EqualTo(0f).Within(0.0001f));
    }
}
