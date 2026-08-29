using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] protected Slash slashPrefab;
    [SerializeField] Direction direction = Direction.Right;
    public Direction Direction => direction;


    public void MoveForward()
    {
        Move(1);
    }

    public void MoveBack()
    {
        Move(-1);
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
