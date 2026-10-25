using System;
using System.Threading;
using UnityEngine;

namespace Duel.GameStates
{
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
#if UNITY_EDITOR
            Debug.Log("OnEnter : " + GetType());
#endif
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
}
