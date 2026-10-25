using System.Threading;
using UnityEngine;

namespace Duel.GameStates
{
    public class GameState_Lose : GameStateBase
    {
        readonly float displayTime = 3f;


        public GameState_Lose(GameStateMachine machine, GameContext context) : base(machine, context) {}

        protected override void OnEnter()
        {
            RunFlow(StateFlow);
        }

        async Awaitable StateFlow(CancellationToken token)
        {
            Context.EnemyStateMachine.DeactivateState();
            Context.UI.RoundResult.Show(isWin: false, Context.RoundTimer.RoundElapsed);
            await Awaitable.WaitForSecondsAsync(displayTime, token);
            Machine.ChangeState(new GameState_TotalResult(Machine, Context));
        }

        protected override void OnExit()
        {
            Context.UI.RoundResult.Hide();
        }
    }
}
