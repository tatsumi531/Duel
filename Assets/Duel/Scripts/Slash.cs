using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] GameObject clashEffectPrefab;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] float hitEffectOffsetRange = 1.5f;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip clashSound;
    readonly float lifeTime = 0.1f;

    CharacterStateMachine owner;
    Slash touchedSlash;
    Vector3 clashEffectPosition;
    CharacterStateMachine touchedCharacter;
    bool resolved;


    public void SetOwner(CharacterStateMachine owner)
    {
        this.owner = owner;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (resolved) {
            return;
        }
        if (collider.TryGetComponent<Slash>(out var otherSlash)) {
            if (otherSlash.owner != owner) {
                touchedSlash = otherSlash;
                var selfCollider = GetComponent<Collider2D>();
                Vector2 selfPos = selfCollider.bounds.center;
                Vector2 otherPos = collider.bounds.center;
                clashEffectPosition = (selfPos + otherPos) * 0.5f;
            }
            return;
        }
        if (collider.TryGetComponent<CharacterStateMachine>(out var target) && target != owner) {
            touchedCharacter = target;
        }
    }

    void LateUpdate()
    {
        if (resolved) {
            return;
        }
        // 相手のSlashにも触れていた場合は、キャラクターへの命中よりも斬撃の相殺(KnockBack)を優先する
        if (touchedSlash != null) {
            ResolveClash(touchedSlash);
            return;
        }

        if (touchedCharacter != null) {
            ResolveHit(touchedCharacter);
        }
    }

    void ResolveClash(Slash other)
    {
        if (other.resolved) {
            return;
        }
        resolved = true;
        other.resolved = true;
        owner.NotifyKnockBack();
        other.owner.NotifyKnockBack();
        var spawned = Instantiate(clashEffectPrefab);
        spawned.transform.position = clashEffectPosition;
        SoundPlayer.PlayOneShot(clashSound);
        Destroy(gameObject);
        Destroy(other.gameObject);
    }

    void ResolveHit(CharacterStateMachine target)
    {
        resolved = true;
        target.NotifyHit();
        var offset = new Vector3(Random.Range(-hitEffectOffsetRange, hitEffectOffsetRange), Random.Range(-hitEffectOffsetRange, hitEffectOffsetRange), 0f);
        var spawned = Instantiate(hitEffectPrefab);
        spawned.transform.position = target.transform.position + offset;
        SoundPlayer.PlayOneShot(hitSound);
        Destroy(gameObject);
    }
}
