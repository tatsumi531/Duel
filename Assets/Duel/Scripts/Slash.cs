using System.Collections;
using UnityEngine;

public class Slash : MonoBehaviour
{
    readonly float lifeTime = 1f;


    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
