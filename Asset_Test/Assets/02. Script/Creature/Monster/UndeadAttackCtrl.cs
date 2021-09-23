using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadAttackCtrl : MonoBehaviour
{
    GameObject player;
    bool isAttacked;

    float attackDamage = 50f;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
        isAttacked = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isAttacked)
            {
                var playerInfo = player.GetComponent<PlayerInfo>();
                playerInfo.Hit(attackDamage);
                isAttacked = true;

                if (playerInfo.curHp <= 0)
                    playerInfo.Die();
            }
        }
    }
    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.SetActive(false);
    }

}
