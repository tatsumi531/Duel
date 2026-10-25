using UnityEngine;

namespace Duel.Characters
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        [Header("アニメーション")]
        public AnimatorOverrideController animatorOverrideController;
        public float spriteOffsetY = 0f;
        
        [Header("AIプロファイル")]
        public CharacterAIProfile aiProfile;

        [Header("パラメータ")]
        public float moveForwardSpeed = 4f;
        public float moveBackSpeed = 2.5f;
        [Header("攻撃エフェクト位置")]
        [Tooltip("攻撃エフェクトを出す前方オフセット距離")]
        public float attackEffectOffset = 1.5f;
    }
}
