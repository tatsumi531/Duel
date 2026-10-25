using unityroom.Api;

namespace Duel.GameStates
{
    public class GameState_TotalResult : GameStateBase
    {
        public GameState_TotalResult(GameStateMachine machine, GameContext context) : base(machine, context) {}

        protected override void OnEnter()
        {
            Context.UI.TotalResult.OnClicked += OnClicked;
            bool isAllWin = Context.RoundCounter.WinRoundCount == Context.GameData.maxRounds;
            Context.UI.TotalResult.Show(isAllWin, Context.RoundCounter.WinRoundCount, Context.RoundTimer.TotalElapsed);
            if (isAllWin) {
                UnityroomApiClient.Instance.SendScore(1, Context.RoundTimer.TotalElapsed, ScoreboardWriteMode.HighScoreDesc);
            }
        }

        void OnClicked()
        {
            Machine.ChangeState(new GameState_TitleScreen(Machine, Context));
        }

        protected override void OnExit()
        {
            Context.UI.TotalResult.OnClicked -= OnClicked;
            Context.UI.TotalResult.Hide();
            Context.RoundTimer.ResetRound();
            Context.RoundTimer.ResetTotal();
            Context.RoundCounter.ResetAll();
        }
    }
}
