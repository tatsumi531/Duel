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
        FadeScreen.Instance.StartFadeIn(ExitToken);
    }
}