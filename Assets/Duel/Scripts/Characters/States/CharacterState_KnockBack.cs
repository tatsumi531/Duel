using DG.Tweening;

namespace Duel.Characters.States
{
    public class CharacterState_KnockBack : CharacterStateBase
    {
        Tween knockbackTween;
        const float knockBackDistance = 2f;
        const float knockBackDuration = 0.5f;


        public CharacterState_KnockBack(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

        public override void OnEnter()
        {
            Character.AnimatorController.SetKnockBackTrigger();
            float targetX = Character.ClampX(Character.transform.position.x - (int)Character.Direction * knockBackDistance);
            knockbackTween = Character.transform.DOMoveX(targetX, knockBackDuration).SetEase(Ease.OutQuad);
            knockbackTween.OnComplete(() =>{ Machine.ChangeState(new CharacterState_Idle(Machine, Context));});
            knockbackTween.Play();
        }

        public override void OnExit()
        {
            knockbackTween?.Kill();
            knockbackTween = null;
            Context.Character.AnimatorController.SetRecoveryTrigger();
        }
    }
}
