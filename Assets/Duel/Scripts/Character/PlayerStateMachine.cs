using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveIntent
{
    None,
    Forward,
    Back
}


// フレーム単位の入力・共有情報。ステート間で受け渡す。
public class PlayerContext
{
    public Character Character { get; }
    public MoveIntent MoveIntent { get; set; }
    public bool AttackTriggered { get; set; }

    public PlayerContext(Character character)
    {
        Character = character;
    }
}


public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] Character character;

    PlayerContext context;
    IPlayerState current;


    void Awake()
    {
        if (character == null)
        {
            character = GetComponent<Character>();
        }
        context = new PlayerContext(character);
    }

    void Start()
    {
        ChangeState(new PlayerState_Neutral(this, context));
    }

    void Update()
    {
        ReadInput();
        current?.OnUpdate();
    }

    public void ChangeState(IPlayerState nextState)
    {
        current?.OnExit();
        current = nextState;
        current.OnEnter();
    }

    public void OnRoundStart()
    {
        ChangeState(new PlayerState_Idle(this, context));
    }

    void ReadInput()
    {
        context.MoveIntent = MoveIntent.None;
        context.AttackTriggered = false;

        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            // A: -X 方向 / D: +X 方向 のワールド上の入力方向
            int worldDirection = 0;
            if (keyboard.aKey.isPressed) worldDirection -= 1;
            if (keyboard.dKey.isPressed) worldDirection += 1;

            if (worldDirection != 0)
            {
                context.MoveIntent = worldDirection == (int)character.Direction
                    ? MoveIntent.Forward
                    : MoveIntent.Back;
            }
        }

        Mouse mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            context.AttackTriggered = true;
        }
    }
}
