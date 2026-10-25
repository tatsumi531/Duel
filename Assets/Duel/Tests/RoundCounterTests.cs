using NUnit.Framework;
using Duel.Core;

public class RoundCounterTests
{
    RoundCounter counter;

    [SetUp]
    public void SetUp()
    {
        counter = new RoundCounter();
    }

    [Test]
    public void 初期状態ではラウンド数と勝利数が0()
    {
        Assert.That(counter.CurrentRound, Is.EqualTo(0));
        Assert.That(counter.WinRoundCount, Is.EqualTo(0));
    }

    [Test]
    public void IncrementRoundでラウンド数が1増える()
    {
        counter.IncrementRound();
        Assert.That(counter.CurrentRound, Is.EqualTo(1));
    }

    [Test]
    public void 最終ラウンドに到達すると次のラウンドは存在しない()
    {
        for (int i = 0; i < 8; i++) {
            counter.IncrementRound();
        }
        Assert.That(counter.ExistNextRound(8), Is.False);
    }

    [Test]
    public void 最終ラウンドの手前では次のラウンドが存在する()
    {
        for (int i = 0; i < 7; i++) {
            counter.IncrementRound();
        }
        Assert.That(counter.ExistNextRound(8), Is.True);
    }

    [Test]
    public void ResetAllでラウンド数と勝利数の両方が0に戻る()
    {
        counter.IncrementRound();
        counter.IncrementWinRound();
        counter.ResetAll();
        Assert.That(counter.CurrentRound, Is.EqualTo(0));
        Assert.That(counter.WinRoundCount, Is.EqualTo(0));
    }
}
