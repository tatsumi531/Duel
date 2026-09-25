using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRoster", menuName = "Scriptable Objects/Enemy Roster")]
public class EnemyRoster : ScriptableObject
{
    public CharacterData[] enemies;

    public CharacterData GetByRound(int round)
    {
        int index = Mathf.Clamp(round - 1, 0, enemies.Length - 1);
        return enemies[index];
    }
}
