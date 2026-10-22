using System.Threading;
using UnityEngine;

public class GameState_RoundIntroduction : GameStateBase
{
    public GameState_RoundIntroduction(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        RunFlow(StateFlow);
    }

    async Awaitable StateFlow(CancellationToken token)
    {
        await Context.UI.Introduction.PlayFlow(token);
        Machine.ChangeState(new GameState_Round(Machine, Context));
    }

    protected override void OnExit()
    {
        Context.UI.Introduction.Initialize();
    }
}
