using UnityEngine;

namespace Duel
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Objects/GameData")]
    public class GameData : ScriptableObject
    {
        public int maxRounds = 8;
    }
}
