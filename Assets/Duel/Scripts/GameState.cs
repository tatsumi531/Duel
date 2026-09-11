using System.Collections;
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
    Coroutine runningFlow;


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
        if (runningFlow != null) {
            Machine.StopCoroutine(runningFlow);
            runningFlow = null;
        }
        OnExit();
    }

    protected virtual void OnEnter() {}
    public virtual void OnUpdate() {}
    protected virtual void OnExit() {}

    protected void RunFlow(IEnumerator flow)
    {
        runningFlow = Machine.StartCoroutine(flow);
    }
}


public class GameState_Boot : GameStateBase
{
    public GameState_Boot(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        FadeScreen.Instance.SetColor(Color.black);
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
        Context.TitleScreen.gameObject.SetActive(true);
        Context.TitleScreen.StartRequested += HandleStartRequested;
        Context.TitleScreen.ExitRequested += HandleExitRequested;
        FadeScreen.Instance.StartFadeIn(ExitToken);
    }

    protected override void OnExit()
    {
        Context.TitleScreen.StartRequested -= HandleStartRequested;
        Context.TitleScreen.ExitRequested -= HandleExitRequested;
        Context.TitleScreen.gameObject.SetActive(false);
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
        RunFlow(StateFlow());
    }

    IEnumerator StateFlow()
    {
        FadeScreen.Instance.StartFadeOut(ExitToken);
        yield return new WaitWhile(() => FadeScreen.Instance.IsFading);
        Context.GameTimer.ResetRound();
        Context.PlayerStateMachine.Initialize(Context.BattleField.PlayerInitialPosition);
        Context.EnemyStateMachine.Initialize(Context.BattleField.EnemyInitialPosition);
        yield return new WaitForSeconds(delaySeconds);
        FadeScreen.Instance.StartFadeIn(ExitToken);
        yield return new WaitWhile(() => FadeScreen.Instance.IsFading);
        Machine.ChangeState(new GameState_RoundIntroduction(Machine, Context));
    }
}


public class GameState_RoundIntroduction : GameStateBase
{
    public GameState_RoundIntroduction(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        RunFlow(StateFlow());
    }

    IEnumerator StateFlow()
    {
        yield return Context.IntroductionScreen.PlayFlow();
        Machine.ChangeState(new GameState_Round(Machine, Context));
    }

    protected override void OnExit()
    {
        Context.IntroductionScreen.Initialize();
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
        Context.GameTimer.OnUpdate();
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
        RunFlow(StateFlow());
    }

    IEnumerator StateFlow()
    {
        Context.ResultScreen.Show(ResultScreen.ShowType.Win, Context.GameTimer.RoundElapsed);
        yield return new WaitForSeconds(displayTime);
        Machine.ChangeState(new GameState_InitializeRound(Machine, Context));
    }

    protected override void OnExit()
    {
        Context.ResultScreen.Hide();
    }
}


public class GameState_Lose : GameStateBase
{
    readonly float displayTime = 3f;


    public GameState_Lose(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        RunFlow(StateFlow());
    }

    IEnumerator StateFlow()
    {
        Context.EnemyStateMachine.DeactivateState();
        Context.ResultScreen.Show(ResultScreen.ShowType.Lose, Context.GameTimer.RoundElapsed);
        yield return new WaitForSeconds(displayTime);
        Machine.ChangeState(new GameState_InitializeRound(Machine, Context));
    }
}


public class GameState_ToNextRound : GameStateBase
{
    public GameState_ToNextRound(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        RunFlow(StateFlow());
    }

    IEnumerator StateFlow()
    {
        FadeScreen.Instance.StartFadeOut(ExitToken);
        yield return new WaitWhile(() => FadeScreen.Instance.IsFading);
        Machine.ChangeState(new GameState_InitializeRound(Machine, Context));
    }
}