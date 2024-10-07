using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAttack : AttackEffect
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        ChooseRandomAttack();
    }
    // randomly select an attack animation out of 3
    void ChooseRandomAttack()
    {
        int randomAttack = Random.Range(0, 3);
        switch (randomAttack)
        {
            case 0:
                animator.SetTrigger("Attack1");
                break;
            case 1:
                animator.SetTrigger("Attack2");
                break;
            case 2:
                animator.SetTrigger("Attack3");
                break;
        }
    }

}
