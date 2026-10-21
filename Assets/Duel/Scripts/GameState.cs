using System;
using System.Threading;
using UnityEngine;

public interface IGameState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}


public abstract class GameStateBase : IGameState
{
    protected GameStateMachine Machine { get; }
    protected GameContext Context { get; }
    protected CancellationToken ExitToken => exitCts.Token;
    CancellationTokenSource exitCts;


    protected GameStateBase(GameStateMachine machine, GameContext context)
    {
        Machine = machine;
        Context = context;
    }

    void IGameState.OnEnter()
    {
        Debug.Log("OnEnter : " + GetType());
        exitCts = new CancellationTokenSource();
        OnEnter();
    }

    void IGameState.OnExit()
    {
        exitCts.Cancel();
        exitCts.Dispose();
        OnExit();
    }

    protected virtual void OnEnter() {}
    public virtual void OnUpdate() {}
    protected virtual void OnExit() {}

    protected void RunFlow(Func<CancellationToken, Awaitable> flow)
    {
        RunFlowAsync(flow);
    }

    async void RunFlowAsync(Func<CancellationToken, Awaitable> flow)
    {
        try {
            await flow(ExitToken);
        }
        catch (OperationCanceledException) {
            // ステート離脱時のキャンセルは正常系。
            // 遷移処理の途中に割り込む形になるため何もしない。
        }
    }
}


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
        Context.UI.RoundResult.Show(ResultScreen.ShowType.Win, Context);
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
        Context.UI.RoundResult.Show(ResultScreen.ShowType.Lose, Context);
        await Awaitable.WaitForSecondsAsync(displayTime, token);
        Machine.ChangeState(new GameState_TotalResult(Machine, Context));
    }

    protected override void OnExit()
    {
        Context.UI.RoundResult.Hide();
    }
}

public class GameState_TotalResult : GameStateBase
{
    public GameState_TotalResult(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        Context.UI.TotalResult.OnClicked += OnClicked;
        ResultScreen.ShowType type = Context.RoundCounter.WinRoundCount == Context.GameData.maxRounds ? ResultScreen.ShowType.Win : ResultScreen.ShowType.Lose;
        Context.UI.TotalResult.Show(type, Context);
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
