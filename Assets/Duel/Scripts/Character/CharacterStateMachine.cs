using UnityEngine;

public abstract class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] protected Character character;
    protected CharacterContext context;
    ICharacterState currentState;
    public bool isAlive = false;

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
        ChangeState(new CharacterState_Neutral(this, context));
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