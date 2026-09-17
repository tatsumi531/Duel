using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour
{
    readonly float lifeTime = 0.1f;

    CharacterStateMachine owner;
    Slash touchedSlash;
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
        Destroy(gameObject);
        Destroy(other.gameObject);
    }

    void ResolveHit(CharacterStateMachine target)
    {
        resolved = true;
        target.NotifyHit();
        Destroy(gameObject);
    }
}
