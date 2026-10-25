using NUnit.Framework;
using Duel.Core;

namespace Duel.Tests
{
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

        [Test]
        public void OnUpdateを複数回呼ぶと経過時間が加算される()
        {
            timer.OnUpdate(0.5f);
            timer.OnUpdate(0.5f);
            timer.OnUpdate(0.5f);
            Assert.That(timer.RoundElapsed, Is.EqualTo(1.5f).Within(0.0001f));
            Assert.That(timer.TotalElapsed, Is.EqualTo(1.5f).Within(0.0001f));
        }
    }
}
