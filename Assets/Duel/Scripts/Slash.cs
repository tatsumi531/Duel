using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour
{
    readonly float lifeTime = 0.4f;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hit : " + collider.gameObject.name);    
    }
}
