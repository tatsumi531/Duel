using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] SpriteRenderer sRenderer;
    public SpriteRenderer SpriteRenderer => sRenderer;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D collider2d;
    public Collider2D Collider2D => collider2d;
    [SerializeField] Slash slashPrefab;
    [SerializeField] AudioClip swordWhip;
    [SerializeField] Direction direction = Direction.Right;
    public Direction Direction => direction;

    CharacterStateMachine stateMachine;
    CharacterData characterData;
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

    public void Initialize(CharacterData characterData)
    {
        this.characterData = characterData;
        sRenderer.transform.localPosition = new Vector3(0f, characterData.spriteOffsetY, 0f);
        AnimatorController.SetOverrideController(characterData.animatorOverrideController);
        transform.position = direction == Direction.Right ? BattleField.LeftInitialPosition : BattleField.RightInitialPosition;
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
        SoundPlayer.PlayOneShot(swordWhip);
    }

    public float GetAttackRange(Character opponent)
    {
        BoxCollider2D slashCollider = slashPrefab.GetComponent<BoxCollider2D>();
        float slashReach = slashCollider.offset.x + slashCollider.size.x * 0.5f;
        float opponentHalfWidth = opponent.Collider2D.bounds.extents.x;
        return characterData.attackEffectOffset + slashReach + opponentHalfWidth;
    }

    void Move(int sign)
    {
        float speed = sign > 0 ? characterData.moveForwardSpeed : characterData.moveBackSpeed;
        float deltaX = sign * (int)direction * speed * Time.deltaTime;
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x + deltaX, BattleField.LeftLimit.x, BattleField.RightLimit.x);
        transform.position = position;
    }
}


public enum Direction
{
    Left = -1,
    Right = 1
}
