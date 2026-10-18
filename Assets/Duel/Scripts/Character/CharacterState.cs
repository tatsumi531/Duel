using DG.Tweening;
using UnityEngine;

public interface ICharacterState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}


public abstract class CharacterStateBase : ICharacterState
{
    protected CharacterStateMachine Machine { get; }
    protected CharacterContext Context { get; }
    protected Character Character => Context.Character;

    protected CharacterStateBase(CharacterStateMachine machine, CharacterContext context)
    {
        Machine = machine;
        Context = context;
    }

    public virtual void OnEnter() {}
    public virtual void OnUpdate() {}
    public virtual void OnExit() {}
}


public class CharacterState_Neutral : CharacterStateBase
{
    public CharacterState_Neutral(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.SpriteRenderer.enabled = true;
        Context.Character.AnimatorController.SetEnabled(true);
        Context.Character.AnimatorController.SetIdleTrigger();
        Context.Character.Collider2D.enabled = true;
    }
}


public class CharacterState_Idle : CharacterStateBase
{
    public CharacterState_Idle(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnUpdate()
    {
        if (Context.AttackTriggered) {
            Machine.ChangeState(new CharacterState_Attack(Machine, Context));
            return;
        }

        switch (Context.MoveIntent) {
            case MoveIntent.Forward:
                Machine.ChangeState(new CharacterState_Forward(Machine, Context));
                break;
            case MoveIntent.Back:
                Machine.ChangeState(new CharacterState_Back(Machine, Context));
                break;
        }
    }
}


public class CharacterState_Forward : CharacterStateBase
{
    public CharacterState_Forward(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.Forward);
    }

    public override void OnUpdate()
    {
        if (Context.AttackTriggered) {
            Machine.ChangeState(new CharacterState_Attack(Machine, Context));
            return;
        }

        if (Context.MoveIntent != MoveIntent.Forward) {
            Machine.ChangeState(new CharacterState_Idle(Machine, Context));
            return;
        }

        Character.MoveForward();
    }

    public override void OnExit()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.None);
    }
}


public class CharacterState_Back : CharacterStateBase
{
    public CharacterState_Back(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.Back);
    }

    public override void OnUpdate()
    {
        if (Context.AttackTriggered) {
            Machine.ChangeState(new CharacterState_Attack(Machine, Context));
            return;
        }

        if (Context.MoveIntent != MoveIntent.Back) {
            Machine.ChangeState(new CharacterState_Idle(Machine, Context));
            return;
        }

        Character.MoveBack();
    }

    public override void OnExit()
    {
        Context.Character.AnimatorController.SetMoveIntent(MoveIntent.None);
    }
}


public class CharacterState_Attack : CharacterStateBase
{
    public CharacterState_Attack(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.AttackAnimationEnded = false;
        Character.AnimatorController.SetAttackTrigger();
    }

    public override void OnUpdate()
    {
        if (Context.AttackAnimationEnded) {
            Machine.ChangeState(new CharacterState_Idle(Machine, Context));
        }
    }
}


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


public class CharacterState_Hit : CharacterStateBase
{
    const float duration = 1f;
    float elapsed;

    public CharacterState_Hit(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        elapsed = 0f;
        Context.Character.PlaySliceEffect();
        Context.Character.AnimatorController.SetEnabled(false);
        Context.Character.Collider2D.enabled = false;
    }

    public override void OnUpdate()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration) {
            Machine.ChangeState(new CharacterState_Dead(Machine, Context));
        }
    }
}


public class CharacterState_Dead : CharacterStateBase
{
    public CharacterState_Dead(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}
}


public class CharacterState_Win : CharacterStateBase
{
    public CharacterState_Win(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}
}