using NUnit.Framework;

public class RoundCounterTests
{
    [Test]
    public void 初期状態ではラウンド数と勝利数が0()
    {
        var counter = new RoundCounter();
        Assert.That(counter.CurrentRound, Is.EqualTo(0));
        Assert.That(counter.WinRoundCount, Is.EqualTo(0));
    }

    [Test]
    public void IncrementRoundでラウンド数が1増える()
    {
        var counter = new RoundCounter();
        counter.IncrementRound();
        Assert.That(counter.CurrentRound, Is.EqualTo(1));
    }

    [Test]
    public void 最終ラウンドに到達すると次のラウンドは存在しない()
    {
        var counter = new RoundCounter();
        for (int i = 0; i < 8; i++) {
            counter.IncrementRound();
        }
        Assert.That(counter.ExistNextRound(8), Is.False);
    }

    [Test]
    public void 最終ラウンドの手前では次のラウンドが存在する()
    {
        var counter = new RoundCounter();
        for (int i = 0; i < 7; i++) {
            counter.IncrementRound();
        }
        Assert.That(counter.ExistNextRound(8), Is.True);
    }

    [Test]
    public void ResetAllでラウンド数と勝利数の両方が0に戻る()
    {
        var counter = new RoundCounter();
        counter.IncrementRound();
        counter.IncrementWinRound();
        counter.ResetAll();
        Assert.That(counter.CurrentRound, Is.EqualTo(0));
        Assert.That(counter.WinRoundCount, Is.EqualTo(0));
    }
}
