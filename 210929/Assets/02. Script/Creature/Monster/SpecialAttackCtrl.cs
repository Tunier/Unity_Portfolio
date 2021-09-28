using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackCtrl : MonoBehaviour
{
    public MonsterGoblinKing goblinKing;
    GameObject player;

    float delayTime = 0;
    Rigidbody rb;

    Vector3 shotRot;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        delayTime = 0;
        shotRot = (new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) - new Vector3(transform.position.x, 0.1f, transform.position.z)).normalized;
    }

    void Update()
    {
        BackPooling();
    }

    private void FixedUpdate()
    {
        rb.AddForce(shotRot * 1f, ForceMode.Impulse);
    }

    public void BackPooling()
    {
        delayTime += Time.deltaTime;
        if (delayTime >= 1f)
        {
            delayTime -= delayTime;
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerCreature = player.GetComponent<Creature>();
            playerCreature.Hit(goblinKing.finalNormalAtk);

            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }
}
