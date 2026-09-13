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
        Context.Character.SpriteRenderer.color = Color.white;
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
        Context.Character.SetMoveIntent(MoveIntent.Forward);
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
        Context.Character.SetMoveIntent(MoveIntent.None);
    }
}


public class CharacterState_Back : CharacterStateBase
{
    public CharacterState_Back(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        Context.Character.SetMoveIntent(MoveIntent.Back);
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
        Context.Character.SetMoveIntent(MoveIntent.None);
    }
}


public class CharacterState_Attack : CharacterStateBase
{
    const float duration = 0.4f;
    float elapsed;

    public CharacterState_Attack(CharacterStateMachine machine, CharacterContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        elapsed = 0f;
        Character.Attack();
    }

    public override void OnUpdate()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration) {
            Machine.ChangeState(new CharacterState_Idle(Machine, Context));
        }
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
