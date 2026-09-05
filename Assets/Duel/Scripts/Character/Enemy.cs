using UnityEngine;

public class Enemy : Character
{
    public bool isAlive = true;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        isAlive = false;
    }

}
