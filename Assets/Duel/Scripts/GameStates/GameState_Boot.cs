using UnityEngine;

namespace Duel.GameStates
{
    public class GameState_Boot : GameStateBase
    {
        public GameState_Boot(GameStateMachine machine, GameContext context) : base(machine, context) {}

        protected override void OnEnter()
        {
            Context.UI.Fade.SetColor(Color.black);
            Context.PlayerStateMachine.OnGameBoot(Context.Enemy);
            Context.EnemyStateMachine.OnGameBoot(Context.Player);
        }

        public override void OnUpdate()
        {
            Machine.ChangeState(new GameState_TitleScreen(Machine, Context));
        }
    }
}
