namespace Duel.Core
{
    public struct AIDecisionInput
    {
        public float distance;              // 相手との距離
        public float selfAttackRange;       // 自分の攻撃が届く距離
        public float opponentAttackRange;   // 相手の攻撃が届く距離
        public float roll;                  // 抽選値
        public bool rushing;                // 距離を詰めると決めた状態かどうか
    }


    public struct AIDecisionParameters
    {
        public float preferredDistanceMargin;   // 相手の攻撃が届く距離からさらにどれだけ離れた位置を保とうとするか
        public float distanceTolerance;         // 維持したい間合いからのズレの許容幅
        public float attackChance;              // 攻撃を行う割合
        public float retreatChance;             // 後ろに下がる割合
        public float approachChance;            // 接近する割合
    }


    public struct AIDecisionResult
    {
        public MoveIntent moveIntent;
        public bool attack;
        public bool rushing;

        public AIDecisionResult(MoveIntent moveIntent, bool attack, bool rushing)
        {
            this.moveIntent = moveIntent;
            this.attack = attack;
            this.rushing = rushing;
        }
    }


    public static class AIDecision
    {
        public static AIDecisionResult Decide(AIDecisionInput input, AIDecisionParameters parameters)
        {
            // 相手が自身の攻撃範囲内にいる場合。
            if (input.distance <= input.selfAttackRange) {
                // 前進フラグは解除し、roll値に応じた行動判断。
                if (input.roll < parameters.retreatChance) {
                    return new AIDecisionResult(MoveIntent.Back, false, false);
                }
                if (input.roll < parameters.retreatChance + parameters.attackChance) {
                    return new AIDecisionResult(MoveIntent.None, true, false);
                }
                return new AIDecisionResult(MoveIntent.None, false, false);
            }
            
            // 一度「詰める」と決めたら、攻撃範囲に入るまでは前進をやめない。
            // これが無いと、安全な間合いと自分の攻撃範囲の間(=相手の攻撃は届くが自分はまだ届かない
            // 危険地帯)を通過する際に毎回判断をやり直してしまい詰めきれないため。
            if (input.rushing) {
                return new AIDecisionResult(MoveIntent.Forward, false, true);
            }
            
            // 相手が自身の攻撃範囲外にいる場合。
            // 相手の攻撃が届く距離にマージンを足した間合いを基準に動く。
            float preferredDistance = input.opponentAttackRange + parameters.preferredDistanceMargin;
            float diff = input.distance - preferredDistance;
            // 維持したい間合いとの差に応じて分岐。
            if (diff > parameters.distanceTolerance) {
                // 間合いより遠い場合は近づく。
                return new AIDecisionResult(MoveIntent.Forward, false, false);
            }
            if (diff < -parameters.distanceTolerance) {
                // 間合いより近い場合は離れる。
                return new AIDecisionResult(MoveIntent.Back, false, false);
            }

            // ちょうど良い間合いにいる場合。
            if (input.roll < parameters.retreatChance) {
                return new AIDecisionResult(MoveIntent.Back, false, false);
            }
            if (input.roll < parameters.retreatChance + parameters.approachChance) {
                // 自身の攻撃範囲まで近づくため前進フラグを立てる。
                return new AIDecisionResult(MoveIntent.Forward, false, true);
            }
            return new AIDecisionResult(MoveIntent.None, false, false);
        }
    }
}
