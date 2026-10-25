using UnityEngine;

namespace Duel
{
    public class BattleField : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("中心からキャラクターの初期位置までの距離")]
        float initialDistance = 5f;

        [SerializeField]
        [Tooltip("中心から移動可能な端までの距離。カメラの表示半幅からキャラの半幅を引いた値を超えないこと")]
        float moveableHalfWidth = 7.3f;

        public Vector3 LeftInitialPosition => new(-initialDistance, 0f, 0f);
        public Vector3 RightInitialPosition => new(initialDistance, 0f, 0f);
        public float LeftLimit => -moveableHalfWidth;
        public float RightLimit => moveableHalfWidth;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            DrawVerticallLine(LeftLimit);
            DrawVerticallLine(RightLimit);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(LeftInitialPosition, 0.25f);
            Gizmos.DrawWireSphere(RightInitialPosition, 0.25f);
        }

        void DrawVerticallLine(float x)
        {
            Gizmos.DrawLine(new Vector3(x, -1f, 0f), new Vector3(x, 4f, 0f));
        }
#endif
    }
}
