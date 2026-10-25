using DG.Tweening;

public class CharacterState_KnockBack : CharacterStateBase
{
    Tween knockbackTween;


    public CharacterState_KnockBack(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.AnimatorController.SetKnockBackTrigger();
        knockbackTween = Context.Character.transform.DOLocalMoveX(-(int)Context.Character.Direction * 2f, 0.5f).SetRelative().SetEase(Ease.OutQuad);
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
