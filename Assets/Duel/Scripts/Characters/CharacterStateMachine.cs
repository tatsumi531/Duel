using UnityEngine;
using Duel.Characters.States;

namespace Duel.Characters
{
    [RequireComponent(typeof(Character))]
    public class CharacterStateMachine : MonoBehaviour
    {
        CharacterContext context;
        ICharacterController controller;
        ICharacterState currentState;
        public bool HasHit { get { return context.HasHit;} }


        public void OnGameBoot(Character opponent)
        {
            context = new CharacterContext(GetComponent<Character>(), opponent);
        }

        public void Initialize(CharacterAIProfile aiProfile = null)
        {
            controller = aiProfile == null ? new PlayerController(context) : new AIController(context, aiProfile);
            context.HasHit = false;
            ChangeState(new CharacterState_Neutral(this, context));
        }

        void Update()
        {
            controller?.OnUpdate();
            currentState?.OnUpdate();
        }

        public void ChangeState(ICharacterState nextState)
        {
            currentState?.OnExit();
            currentState = nextState;
#if UNITY_EDITOR
            Debug.Log(GetType() + " : OnEnter : " + nextState.GetType());
#endif
            currentState.OnEnter();
        }

        public void ActivateState()
        {
            ChangeState(new CharacterState_Idle(this, context));
        }

        public void DeactivateState()
        {
            ChangeState(new CharacterState_NoneActive(this, context));
        }

        public void NotifyHit()
        {
            if (!context.HasHit) {
                context.HasHit = true;
                ChangeState(new CharacterState_Hit(this, context));
            }
        }

        public void NotifyAttackAnimationEnd()
        {
            context.AttackAnimationEnded = true;
        }

        public void NotifyKnockBack()
        {
            if (!context.HasHit) {
                ChangeState(new CharacterState_KnockBack(this, context));
            }
        }
    }
}
