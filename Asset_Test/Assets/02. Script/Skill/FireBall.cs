using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    GameObject fireBallEffect;
    [SerializeField]
    GameObject explosion_Effet;

    public PlayerInfo player;

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

            // ���� ��Ʈ����
            if (other.CompareTag("Monster"))
            {
                var mob = other.GetComponent<MonsterBase>();

                var _skill = SkillDatabase.instance.AllSkillDic["0300000"];

                mob.Hit(_skill.Value + (player.player_Skill_Dic[_skill.UIDCODE] - 1) * _skill.ValueFactor);
            }

            Destroy(gameObject, 1f);
        }
    }

    // ���߿� ������Ʈ Ǯ���Ҷ� ����
    IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }
}
