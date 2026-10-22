using System.Threading;
using UnityEngine;

public class GameState_InitializeRound : GameStateBase
{
    readonly float delaySeconds = 1f;


    public GameState_InitializeRound(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        RunFlow(StateFlow);
    }

    async Awaitable StateFlow(CancellationToken token)
    {
        Context.RoundTimer.ResetRound();
        Context.RoundCounter.IncrementRound();
        await Context.UI.Fade.FadeOutAsync(token);
        Context.Player.Initialize(Context.PlayerCharacterData);
        Context.PlayerStateMachine.Initialize();
        CharacterData enemyData = Context.EnemyRoster.GetByRound(Context.RoundCounter.CurrentRound);
        Context.Enemy.Initialize(enemyData);
        Context.EnemyStateMachine.Initialize(enemyData.aiProfile);
        await Awaitable.WaitForSecondsAsync(delaySeconds, token);
        await Context.UI.Fade.FadeInAsync(token);
        Machine.ChangeState(new GameState_RoundIntroduction(Machine, Context));
    }
}
