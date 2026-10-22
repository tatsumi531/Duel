using UnityEngine;

public class GameState_Round : GameStateBase
{
    public GameState_Round(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        Context.PlayerStateMachine.ActivateState();
        Context.EnemyStateMachine.ActivateState();
    }

    public override void OnUpdate()
    {
        Context.RoundTimer.OnUpdate(Time.deltaTime);
        if (Context.EnemyStateMachine.HasHit) {
            Machine.ChangeState(new GameState_Win(Machine, Context));
            return;
        }
        if (Context.PlayerStateMachine.HasHit) {
            Machine.ChangeState(new GameState_Lose(Machine, Context));
            return;
        }
    }
}
