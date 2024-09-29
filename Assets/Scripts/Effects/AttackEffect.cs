using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    [SerializeField] private float duration = 1;
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
