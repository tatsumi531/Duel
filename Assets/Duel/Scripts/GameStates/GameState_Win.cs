using System.Threading;
using UnityEngine;

namespace Duel.GameStates
{
    public class GameState_Win : GameStateBase
    {
        readonly float displayTime = 3f;


        public GameState_Win(GameStateMachine machine, GameContext context) : base(machine, context) {}

        protected override void OnEnter()
        {
            RunFlow(StateFlow);
        }

        async Awaitable StateFlow(CancellationToken token)
        {
            Context.PlayerStateMachine.DeactivateState();
            Context.RoundCounter.IncrementWinRound();
            Context.UI.RoundResult.Show(isWin: true, Context.RoundTimer.RoundElapsed);
            await Awaitable.WaitForSecondsAsync(displayTime, token);

            if (Context.RoundCounter.ExistNextRound(Context.GameData.maxRounds)) {
                Machine.ChangeState(new GameState_InitializeRound(Machine, Context));
            } else {
                Machine.ChangeState(new GameState_TotalResult(Machine, Context));
            }
        }

        protected override void OnExit()
        {
            Context.UI.RoundResult.Hide();
        }
    }
}
