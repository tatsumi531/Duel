using UnityEngine;

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
        Debug.Log(GetType() + " : OnEnter : " + nextState.GetType());
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


public class CharacterContext
{
    public Character Character { get; }
    public Character Opponent { get; }
    public MoveIntent MoveIntent { get; set; }
    public bool AttackTriggered { get; set; }
    public bool AttackAnimationEnded { get; set; }
    public bool HasHit { get; set; }


    public CharacterContext(Character character, Character opponent)
    {
        Character = character;
        Opponent = opponent;
    }
}


public enum MoveIntent
{
    None = 0,
    Forward = 1,
    Back = 2
    // Animatorのパラメータと連動
}
