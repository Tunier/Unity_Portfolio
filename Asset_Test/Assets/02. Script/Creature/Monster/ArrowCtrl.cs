using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    GameObject player;

    public float arrowDamage = 10f;
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
        if (delayTime >= 2f)
        {
            delayTime -= delayTime;
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);
            //WeaponManager.instance.arrowPool.Add(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playerInfo = player.GetComponent<PlayerInfo>();

            //캐릭터 히트함수
            playerInfo.Hit(arrowDamage);
            rb.velocity = Vector3.zero;
            this.gameObject.SetActive(false);

            if (playerInfo.curHp <= 0)
                playerInfo.Die();
        }
    }
}
