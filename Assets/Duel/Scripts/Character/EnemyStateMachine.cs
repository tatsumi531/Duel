using UnityEngine;

public class EnemyStateMachine : CharacterStateMachine
{
    CharacterAIProfile profile;
    float decisionTimer;
    MoveIntent currentMoveIntent = MoveIntent.None;
    bool rushing; // 間合いを詰めると決めた後、攻撃範囲に入るまで前進をやめないためのフラグ


    public void SetProfile(CharacterAIProfile profile)
    {
        this.profile = profile;
    }

    public override void InputAction()
    {
        context.AttackTriggered = false;

        if (profile == null) return;

        decisionTimer -= Time.deltaTime;
        if (decisionTimer <= 0f) {
            MakeDecision();
            decisionTimer = Random.Range(profile.reactionTimeMin, profile.reactionTimeMax);
        }

        context.MoveIntent = currentMoveIntent;
    }

    void MakeDecision()
    {
        float distance = Mathf.Abs(context.Opponent.transform.position.x - context.Character.transform.position.x);
        float attackRange = context.Character.GetAttackRange(context.Opponent);
        float roll = Random.value;

        // 相手が自身の攻撃範囲内にいる場合。
        if (distance <= attackRange) {
            // 前進フラグは解除し、通常の行動判断に戻す。
            rushing = false;
            // roll値とプロファイルの値に応じて、後退、攻撃、足踏みに分岐。
            if (roll < profile.retreatChance) {
                currentMoveIntent = MoveIntent.Back;
            } else if (roll < profile.retreatChance + profile.attackChance) {
                context.AttackTriggered = true;
                currentMoveIntent = MoveIntent.None;
            } else {
                currentMoveIntent = MoveIntent.None;
            }
            return;
        }

        // 一度「詰める」と決めたら、攻撃範囲に入るまでは前進をやめない。
        // これが無いと、安全な間合いと自分の攻撃範囲の間(=相手の攻撃は届くが自分はまだ届かない
        // 危険地帯)を通過する際に毎回判断をやり直してしまい詰めきれないため。
        if (rushing) {
            currentMoveIntent = MoveIntent.Forward;
            return;
        }

        // 相手が自身の攻撃範囲外にいる場合。
        // 相手の攻撃が届く距離にマージンを足した間合いを基準に動く。
        // 自分の攻撃範囲を基準にすると、相手の攻撃範囲の方が広い場合に、「間合いを保っているつもりが相手の攻撃には当たる」状態になってしまうため。
        // 相手の攻撃範囲を基準にすることで、相手の攻撃が届かない間合いを保とうとする。
        float opponentAttackRange = context.Opponent.GetAttackRange(context.Character);
        float preferredDistance = opponentAttackRange + profile.preferredDistanceMargin;
        float diff = distance - preferredDistance;

        // 維持したい間合いとの差に応じて分岐。
        if (diff > profile.distanceTolerance) {
            // 間合いより遠い場合は近づく。
            currentMoveIntent = MoveIntent.Forward;
        } else if (diff < -profile.distanceTolerance) {
            // 間合いより近い場合は離れる。
            currentMoveIntent = MoveIntent.Back;
        } else {

            // ちょうど良い間合いにいる場合。
            if (roll < profile.retreatChance) {
                currentMoveIntent = MoveIntent.Back;
            } else if (roll < profile.retreatChance + profile.approachChance) {
                // 自身の攻撃範囲まで近づくため前進フラグを立てる。
                rushing = true;
                currentMoveIntent = MoveIntent.Forward;
            } else {
                currentMoveIntent = MoveIntent.None;
            }
        }
    }
}
