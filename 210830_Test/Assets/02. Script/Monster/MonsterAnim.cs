using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnim : MonoBehaviour
{
    Animator animator;

    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashDie = Animator.StringToHash("IsDie");
    readonly int hashHit = Animator.StringToHash("IsHit");
    readonly int hashAttack = Animator.StringToHash("IsAttack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnMove(bool _true)
    {
        animator.SetBool(hashMove, _true);
    }
    public void OnDie()
    {
        animator.SetTrigger(hashDie);
    }

    public void OnAttack()
    {
        animator.SetTrigger(hashAttack);
    }

    public void OnHit()
    {
        animator.SetTrigger(hashHit);
    }
}

