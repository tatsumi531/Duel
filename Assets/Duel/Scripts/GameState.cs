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
        Machine.ChangeState(new GameState_Round(Machine, Context));
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


public class GameState_StartRound : GameStateBase
{
    public GameState_StartRound(GameStateMachine machine, GameContext context) : base(machine, context) {}


}


public class GameState_Round : GameStateBase
{
    float elapsed;

    public GameState_Round(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        elapsed = 0f;
        Context.PlayerStateMachine.OnRoundStart();
        Context.EnemyStateMachine.OnRoundStart();
    }

    public override void OnUpdate()
    {
        elapsed += Time.deltaTime;
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
    float elapsed = 0f;
    float displayTime = 3f;


    public GameState_Win(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        elapsed = 0f;
        Context.ResultScreen.Show(ResultScreen.ShowType.Win, 10f);
    }

    public override void OnUpdate()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= displayTime) {
            Machine.ChangeState(new GameState_ToNextRound(Machine, Context));
        }
    }
}


public class GameState_Lose : GameStateBase
{
    public GameState_Lose(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        
    }


}


public class GameState_ToNextRound : GameStateBase
{
    public GameState_ToNextRound(GameStateMachine machine, GameContext context) : base(machine, context) {}

    protected override void OnEnter()
    {
        Machine.StartCoroutine(StateFlow());
    }

    IEnumerator StateFlow()
    {
        FadeScreen.Instance.StartFadeOut(ExitToken);
        yield return new WaitWhile(() => FadeScreen.Instance.IsFading);
        FadeScreen.Instance.StartFadeIn(ExitToken);
        yield return new WaitWhile(() => FadeScreen.Instance.IsFading);
        Machine.ChangeState(new GameState_StartRound(Machine, Context));
    }
}
