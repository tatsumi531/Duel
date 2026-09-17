using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]SpriteRenderer sRenderer;
    public SpriteRenderer SpriteRenderer => sRenderer;
    [SerializeField]Animator animator;
    [SerializeField] CharacterData characterData;
    [SerializeField] protected Slash slashPrefab;
    [SerializeField] Direction direction = Direction.Right;
    public Direction Direction => direction;

    CharacterStateMachine stateMachine;
    SpriteSliceEffect sliceEffect;
    public CharacterAnimatorController AnimatorController { get; private set; }


    void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
        sliceEffect = GetComponent<SpriteSliceEffect>();
        AnimatorController = new CharacterAnimatorController(animator);
        if (sliceEffect == null) {
            sliceEffect = gameObject.AddComponent<SpriteSliceEffect>();
        }
    }

    public void PlaySliceEffect()
    {
        sliceEffect.Play(sRenderer);
    }

    public void MoveForward()
    {
        Move(1);
    }

    public void MoveBack()
    {
        Move(-1);
    }

    public void Attack()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += characterData.attackEffectOffset * (int)Direction;
        var spawned = Instantiate(slashPrefab, spawnPosition, Quaternion.identity);
        spawned.transform.localScale = new Vector3((int)Direction, 1f, 1f);
        spawned.SetOwner(stateMachine);
    }

    void Move(int sign)
    {
        float speed = sign > 0 ? characterData.moveForwardSpeed : characterData.moveBackSpeed;
        float deltaX = sign * (int)direction * speed * Time.deltaTime;
        transform.Translate(deltaX, 0f, 0f);
    }
}


public enum Direction
{
    Left = -1,
    Right = 1
}
