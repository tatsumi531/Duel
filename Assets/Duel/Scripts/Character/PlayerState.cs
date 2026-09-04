using UnityEngine;

public interface IPlayerState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}


public abstract class PlayerStateBase : IPlayerState
{
    protected PlayerStateMachine Machine { get; }
    protected PlayerContext Context { get; }
    protected Character Character => Context.Character;

    protected PlayerStateBase(PlayerStateMachine machine, PlayerContext context)
    {
        Machine = machine;
        Context = context;
    }

    public virtual void OnEnter() {}
    public virtual void OnUpdate() {}
    public virtual void OnExit() {}
}


// 一切行動をしない状態。入力を読まない。
// Idle への遷移は対戦開始処理など外部から Machine.ChangeState() で行う。
public class PlayerState_Neutral : PlayerStateBase
{
    public PlayerState_Neutral(PlayerStateMachine machine, PlayerContext context) : base(machine, context) {}
}


// 何も行動していない待機状態。入力を見て次のステートへ振り分けるだけ。
public class PlayerState_Idle : PlayerStateBase
{
    public PlayerState_Idle(PlayerStateMachine machine, PlayerContext context) : base(machine, context) {}

    public override void OnUpdate()
    {
        if (Context.AttackTriggered)
        {
            Machine.ChangeState(new PlayerState_Attack(Machine, Context));
            return;
        }

        switch (Context.MoveIntent)
        {
            case MoveIntent.Forward:
                Machine.ChangeState(new PlayerState_Forward(Machine, Context));
                break;
            case MoveIntent.Back:
                Machine.ChangeState(new PlayerState_Back(Machine, Context));
                break;
        }
    }
}


public class PlayerState_Forward : PlayerStateBase
{
    public PlayerState_Forward(PlayerStateMachine machine, PlayerContext context) : base(machine, context) {}

    public override void OnUpdate()
    {
        if (Context.AttackTriggered)
        {
            Machine.ChangeState(new PlayerState_Attack(Machine, Context));
            return;
        }

        if (Context.MoveIntent != MoveIntent.Forward)
        {
            Machine.ChangeState(new PlayerState_Idle(Machine, Context));
            return;
        }

        Character.MoveForward();
    }
}


public class PlayerState_Back : PlayerStateBase
{
    public PlayerState_Back(PlayerStateMachine machine, PlayerContext context) : base(machine, context) {}

    public override void OnUpdate()
    {
        if (Context.AttackTriggered)
        {
            Machine.ChangeState(new PlayerState_Attack(Machine, Context));
            return;
        }

        if (Context.MoveIntent != MoveIntent.Back)
        {
            Machine.ChangeState(new PlayerState_Idle(Machine, Context));
            return;
        }

        Character.MoveBack();
    }
}


public class PlayerState_Attack : PlayerStateBase
{
    const float duration = 0.4f;
    float elapsed;

    public PlayerState_Attack(PlayerStateMachine machine, PlayerContext context) : base(machine, context) {}

    public override void OnEnter()
    {
        elapsed = 0f;
        Character.Attack();
    }

    public override void OnUpdate()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            Machine.ChangeState(new PlayerState_Idle(Machine, Context));
        }
    }
}
