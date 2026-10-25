using UnityEngine;
using Duel.Core;

namespace Duel.Characters
{
    [CreateAssetMenu(fileName = "CharacterAIProfile", menuName = "Scriptable Objects/Character AI Profile")]
    public class CharacterAIProfile : ScriptableObject
    {
        [Header("反応速度のブレ")]
        [Tooltip("次の行動を選び直すまでの間隔(最短)。小さいほど反応が速い")]
        public float reactionTimeMin = 0.2f;
        [Tooltip("次の行動を選び直すまでの間隔(最長)。大きいほど反応が遅い")]
        public float reactionTimeMax = 0.6f;

        [Header("維持したい間合い")]
        [Tooltip("攻撃圏内でないとき、相手(プレイヤー)の攻撃が届く距離からさらにどれだけ離れた位置を保とうとするか。" +
            "0に近いほど相手の攻撃範囲ギリギリまで詰める積極的な性格になり、大きいほど安全に距離を取る慎重な性格になる")]
        public float preferredDistanceMargin = 1f;
        [Tooltip("維持したい間合いからのズレの許容幅")]
        public float distanceTolerance = 0.5f;

        [Header("行動の確率")]
        [Range(0f, 1f)]
        [Tooltip("攻撃圏内に入ったとき、攻撃を選ぶ確率")]
        public float attackChance = 0.6f;
        [Range(0f, 1f)]
        [Tooltip("後ろに下がる行動を選ぶ確率")]
        public float retreatChance = 0.2f;
        [Range(0f, 1f)]
        [Tooltip("維持したい間合いにいるとき、詰めにいく確率")]
        public float approachChance = 0.3f;


        public AIDecisionParameters ToParameters()
        {
            AIDecisionParameters param = new AIDecisionParameters();
            param.preferredDistanceMargin = preferredDistanceMargin;
            param.distanceTolerance = distanceTolerance;
            param.attackChance = attackChance;
            param.retreatChance = retreatChance;
            param.approachChance = approachChance;
            return param;
        }
    }
}
