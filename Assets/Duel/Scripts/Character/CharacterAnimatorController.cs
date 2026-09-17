using UnityEngine;

public class CharacterAnimatorController
{
    readonly Animator animator;
    readonly int moveIntentHash = Animator.StringToHash("MoveIntent");
    readonly int attackTriggerHash = Animator.StringToHash("AttackTrigger");
    readonly int knockBackTriggerHash = Animator.StringToHash("KnockBackTrigger");
    readonly int recoveryTriggerHash = Animator.StringToHash("RecoveryTrigger");


    public void SetEnabled(bool enabled)
    {
        animator.enabled = enabled;
    }
    
    public CharacterAnimatorController(Animator animator)
    {
        this.animator = animator;
    }

    public void SetMoveIntent(MoveIntent intent)
    {
        animator.SetInteger(moveIntentHash, (int)intent);
    }

    public void SetAttackTrigger()
    {
        animator.SetTrigger(attackTriggerHash);
    }

    public void SetKnockBackTrigger()
    {
        animator.SetTrigger(knockBackTriggerHash);
    }

    public void SetRecoveryTrigger()
    {
        animator.SetTrigger(recoveryTriggerHash);
    }
}
