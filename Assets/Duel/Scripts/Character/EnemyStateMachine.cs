using UnityEngine;

public class EnemyStateMachine : CharacterStateMachine
{
    CharacterAIProfile profile;
    float decisionTimer;
    MoveIntent currentMoveIntent = MoveIntent.None;


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
        float roll = Random.value;

        if (distance <= profile.attackRange) {
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

        // 攻撃圏外では、維持したい間合いを基準に動く。
        float diff = distance - profile.preferredDistance;

        if (diff > profile.distanceTolerance) {
            currentMoveIntent = MoveIntent.Forward;
        } else if (diff < -profile.distanceTolerance) {
            currentMoveIntent = MoveIntent.Back;
        } else {
            // 基本は間合いを維持しつつ、詰めたり下がったりして揺さぶる動き。
            if (roll < profile.retreatChance) {
                currentMoveIntent = MoveIntent.Back;
            } else if (roll < profile.retreatChance + profile.approachChance) {
                currentMoveIntent = MoveIntent.Forward;
            } else {
                currentMoveIntent = MoveIntent.None;
            }
        }
    }
}
