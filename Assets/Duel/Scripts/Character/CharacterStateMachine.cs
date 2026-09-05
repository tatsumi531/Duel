using UnityEngine;

public abstract class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] protected Character character;
    protected CharacterContext context;
    ICharacterState currentState;

    public bool HasHit { get; private set; }


    public abstract void InputAction();

    void Awake()
    {
        context = new CharacterContext(character);
    }

    void Update()
    {
        InputAction();
        currentState?.OnUpdate();
    }

    public void ChangeState(ICharacterState nextState)
    {
        currentState?.OnExit();
        currentState = nextState;
        currentState.OnEnter();
    }

    public void OnRoundStart()
    {
        ChangeState(new CharacterState_Idle(this, context));
    }

    public void NotifyHit()
    {
        HasHit = true;
        ChangeState(new CharacterState_Hit(this, context));
    }
}


public class CharacterContext
{
    public Character Character { get; }
    public MoveIntent MoveIntent { get; set; }
    public bool AttackTriggered { get; set; }

    public CharacterContext(Character character)
    {
        Character = character;
    }
}


public enum MoveIntent
{
    None,
    Forward,
    Back
}
