using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterAnimationEventReceiver : MonoBehaviour
{
    Character character;


    void Awake()
    {
        character = GetComponent<Character>();
    }

    public void OnAttack()
    {
        character.Attack();
    }
}
