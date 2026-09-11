using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]SpriteRenderer sRenderer;
    public SpriteRenderer SpriteRenderer => sRenderer;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] protected Slash slashPrefab;
    [SerializeField] Direction direction = Direction.Right;
    public Direction Direction => direction;

    CharacterStateMachine stateMachine;

    void Awake()
    {
        stateMachine = GetComponent<CharacterStateMachine>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (stateMachine != null && collider.TryGetComponent<Slash>(out _)) {
            stateMachine.NotifyHit();
        }
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
        spawnPosition.x += 1.5f * (int)Direction;
        var spawned = Instantiate(slashPrefab, spawnPosition, Quaternion.identity);
        spawned.transform.localScale = new Vector3((int)Direction, 1f, 1f);
    }

    void Move(int sign)
    {
        float deltaX = sign * (int)direction * moveSpeed * Time.deltaTime;
        transform.Translate(deltaX, 0f, 0f);
    }
}


public enum Direction
{
    Left = -1,
    Right = 1
}
