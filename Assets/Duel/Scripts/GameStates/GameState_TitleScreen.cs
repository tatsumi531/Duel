using System.Threading;
using UnityEngine;

namespace Duel.GameStates
{
    public class GameState_TitleScreen : GameStateBase
    {
        public GameState_TitleScreen(GameStateMachine machine, GameContext context) : base(machine, context) {}

        protected override void OnEnter()
        {
            Context.UI.Title.gameObject.SetActive(true);
            Context.UI.Title.StartRequested += HandleStartRequested;
            Context.UI.Title.ExitRequested += HandleExitRequested;
            RunFlow(StateFlow);
        }

        async Awaitable StateFlow(CancellationToken token)
        {
            await Context.UI.Fade.FadeInAsync(token);
        }

        protected override void OnExit()
        {
            Context.UI.Title.StartRequested -= HandleStartRequested;
            Context.UI.Title.ExitRequested -= HandleExitRequested;
            Context.UI.Title.gameObject.SetActive(false);
        }

        void HandleStartRequested()
        {
            Machine.ChangeState(new GameState_InitializeRound(Machine, Context));
        }

        void HandleExitRequested()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
