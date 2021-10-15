using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackCtrl : MonoBehaviour
{
    public MonsterGoblinKing goblinKing;
    GameObject player;

    float delayTime = 0;
    Rigidbody rb;

    bool isAttacked;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        delayTime = 0;
        isAttacked = false;
    }

    void Update()
    {
        delayTime += Time.deltaTime;

        if (delayTime >= 0.5f)
        {
            delayTime -= delayTime;
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * 1f, ForceMode.Impulse);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isAttacked == false)
            {
                isAttacked = true;
                var playerCreature = player.GetComponent<Creature>();
                playerCreature.Hit(goblinKing.finalNormalAtk);
            }
        }
    }
}