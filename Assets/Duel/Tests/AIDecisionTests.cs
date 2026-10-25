using NUnit.Framework;
using Duel.Core;

namespace Duel.Tests
{
    public class AIDecisionTests
    {
        static AIDecisionParameters DefaultParameters() => new AIDecisionParameters {
            preferredDistanceMargin = 1f,
            distanceTolerance = 0.5f,
            attackChance = 0.6f,
            retreatChance = 0.2f,
            approachChance = 0.3f
        };

        [Test]
        public void 攻撃範囲内で抽選値が攻撃の帯に入れば攻撃する()
        {
            var input = new AIDecisionInput {
                distance = 1.5f,
                selfAttackRange = 2f,
                opponentAttackRange = 2f,
                roll = 0.5f,
                rushing = false
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.attack, Is.True);
            Assert.That(result.moveIntent, Is.EqualTo(MoveIntent.None));
        }

        [Test]
        public void 攻撃範囲内で抽選値が後退の帯に入れば後退する()
        {
            var input = new AIDecisionInput {
                distance = 1.5f,
                selfAttackRange = 2f,
                opponentAttackRange = 2f,
                roll = 0.1f,
                rushing = false
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.attack, Is.False);
            Assert.That(result.moveIntent, Is.EqualTo(MoveIntent.Back));
        }

        [Test]
        public void 攻撃範囲に入ったら前進フラグは解除される()
        {
            var input = new AIDecisionInput {
                distance = 1.5f,
                selfAttackRange = 2f,
                opponentAttackRange = 2f,
                roll = 0.5f,
                rushing = true
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.rushing, Is.False);
        }

        [Test]
        public void 詰めると決めた後は前進を続ける()
        {
            var input = new AIDecisionInput {
                distance = 2.5f,
                selfAttackRange = 2f,
                opponentAttackRange = 3f,
                roll = 0.11f,   // どんな値でも関係なく前進するかどうか
                rushing = true
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.rushing, Is.True);
            Assert.That(result.moveIntent, Is.EqualTo(MoveIntent.Forward));
        }

        [Test]
        public void 詰めると決めていなければ危険な間合いからは離れる()
        {
            var input = new AIDecisionInput {
                distance = 2.5f,
                selfAttackRange = 2f,
                opponentAttackRange = 3f,
                roll = 0.11f,
                rushing = false
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.moveIntent, Is.EqualTo(MoveIntent.Back));
        }

        [Test]
        public void 維持したい間合いより遠ければ近づく()
        {
            var input = new AIDecisionInput {
                distance = 10f,             // 維持したい間合いより遠い
                selfAttackRange = 2f,
                opponentAttackRange = 3f,
                roll = 0.5f,
                rushing = false,
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.moveIntent, Is.EqualTo(MoveIntent.Forward));
        }

        [Test]
        public void ちょうど良い間合いで前進を選んだ場合は前進フラグが立つ()
        {
            var input = new AIDecisionInput {
                distance = 4f,
                selfAttackRange = 2f,
                opponentAttackRange = 3f,
                roll = 0.35f,
                rushing = false
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.moveIntent, Is.EqualTo(MoveIntent.Forward));
            Assert.That(result.rushing, Is.True);
        }

        [Test]
        public void 攻撃範囲内で抽選値がどの帯にも入らなければ何もしない()
        {
            var input = new AIDecisionInput {
                distance = 1.5f,
                selfAttackRange = 2f,
                opponentAttackRange = 2f,
                roll = 0.9f,
                rushing = false
            };
            AIDecisionResult result = AIDecision.Decide(input, DefaultParameters());
            Assert.That(result.moveIntent, Is.EqualTo(MoveIntent.None));
            Assert.That(result.attack, Is.False);
        }
    }
}
