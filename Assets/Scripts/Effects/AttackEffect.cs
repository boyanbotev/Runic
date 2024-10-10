using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float duration = 1;
    public float damage = 1;
    public float knockbackForce = 0;
    private void Start()
    {
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
