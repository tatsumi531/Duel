using DG.Tweening;
using UnityEngine;

namespace Duel.Characters
{
    public class SpriteSliceEffect : MonoBehaviour
    {
        const string SliceMaterialResourcePath = "Duel_SpriteSlice";

        static readonly int CutPointId = Shader.PropertyToID("_CutPoint");
        static readonly int CutNormalId = Shader.PropertyToID("_CutNormal");
        static readonly int KeepSideId = Shader.PropertyToID("_KeepSide");

        [SerializeField] Material sliceMaterial;
        [SerializeField] float cutAngle = 20f;
        [SerializeField] float angleJitter = 15f;
        [SerializeField] float positionJitter = 0.15f;
        [SerializeField] float launchSideDistance = 10f;
        [SerializeField] float launchDropDistance = 10f;
        [SerializeField] float spinAngle = 260f;
        [SerializeField] float duration = 1.8f;


        void Awake()
        {
            if (sliceMaterial == null) {
                sliceMaterial = Resources.Load<Material>(SliceMaterialResourcePath);
            }
        }

        public void Play(SpriteRenderer source)
        {
            if (sliceMaterial == null) {
                Debug.LogError($"SpriteSliceEffect: slice material not found at Resources/{SliceMaterialResourcePath}.");
                return;
            }

            source.enabled = false;

            Bounds bounds = source.sprite.bounds;
            float angle = (cutAngle + Random.Range(-angleJitter, angleJitter)) * Mathf.Deg2Rad;
            Vector2 normal = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            Vector2 cutPoint = (Vector2)bounds.center + Random.insideUnitCircle * positionJitter;

            SpawnHalf(source, cutPoint, normal, 1f, normal.y >= 0f);
            SpawnHalf(source, cutPoint, normal, -1f, normal.y < 0f);
        }

        void SpawnHalf(SpriteRenderer source, Vector2 cutPoint, Vector2 normal, float keepSide, bool isLaunched)
        {
            var piece = new GameObject(isLaunched ? "SliceUpper" : "SliceLower");
            piece.transform.SetPositionAndRotation(source.transform.position, source.transform.rotation);
            piece.transform.localScale = source.transform.lossyScale;

            var pieceRenderer = piece.AddComponent<SpriteRenderer>();
            pieceRenderer.sprite = source.sprite;
            pieceRenderer.color = source.color;
            pieceRenderer.sortingLayerID = source.sortingLayerID;
            pieceRenderer.sortingOrder = source.sortingOrder;

            var pieceMaterial = new Material(sliceMaterial);
            pieceMaterial.SetVector(CutPointId, cutPoint);
            pieceMaterial.SetVector(CutNormalId, normal);
            pieceMaterial.SetFloat(KeepSideId, keepSide);
            pieceRenderer.material = pieceMaterial;

            Sequence seq = DOTween.Sequence();
            if (isLaunched) {
                Vector3 launchOffset = (Vector3)(normal * (keepSide * launchSideDistance)) + Vector3.up * (launchSideDistance * 0.5f) + Vector3.down * launchDropDistance;
                seq.Append(piece.transform.DOMove(piece.transform.position + launchOffset, duration).SetEase(Ease.OutCubic));
                seq.Join(piece.transform.DORotate(new Vector3(0f, 0f, spinAngle * keepSide), duration, RotateMode.FastBeyond360));
            }
            seq.Join(pieceRenderer.DOFade(0f, duration * 0.6f).SetDelay(duration * 0.4f));
            seq.OnComplete(() => {
                Destroy(piece);
                Destroy(pieceMaterial);
            });
            seq.Play();
        }
    }
}
