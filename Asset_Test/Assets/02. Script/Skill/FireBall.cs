using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    GameObject fireBallEffect;
    [SerializeField]
    GameObject explosion_Effet;

    Rigidbody rb;

    float moveSpeed;

    private void Awake()
    {
        explosion_Effet.SetActive(false);
        rb = GetComponent<Rigidbody>();

        moveSpeed = 30f;
    }

    void Start()
    {
        rb.velocity = transform.forward * moveSpeed;
        Destroy(gameObject, 1.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("RaycastTarget") && !other.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            fireBallEffect.SetActive(false);
            explosion_Effet.SetActive(true);

            // 몬스터 히트판정

            Destroy(gameObject, 1f);
        }
    }

    // 나중에 오브젝트 풀링할때 구현
    IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false); 
    }
}
