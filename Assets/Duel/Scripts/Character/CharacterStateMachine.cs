using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] Character character;
    [SerializeField] Character opponent;
    protected CharacterContext context;
    ICharacterController controller;
    ICharacterState currentState;
    public bool HasHit { get; private set; }


    void Awake()
    {
        context = new CharacterContext(character, opponent);
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

    public void Initialize(CharacterAIProfile aiProfile = null)
    {
        controller = aiProfile == null ? new PlayerController(context) : new AIController(context, aiProfile);
        HasHit = false;
        ChangeState(new CharacterState_Neutral(this, context));
    }

    public void ActivateState()
    {
        ChangeState(new CharacterState_Idle(this, context));
    }

    public void DeactivateState()
    {
        ChangeState(new CharacterState_Neutral(this, context));
    }

    public void NotifyHit()
    {
        if (!HasHit) {
            HasHit = true;
            ChangeState(new CharacterState_Hit(this, context));
        }
    }

    public void NotifyAttackAnimationEnd()
    {
        context.AttackAnimationEnded = true;
    }

    public void NotifyKnockBack()
    {
        if (!HasHit) {
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
