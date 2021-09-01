using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCtrl : MonoBehaviour
{
    PlayerInfo player;

    [SerializeField]
    Player_SkillIndicator skillIndicator;

    [SerializeField]
    GameObject[] skillprefebs;

    SkillDatabase skillDB;

    CharacterController cController;

    void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();
        skillDB = FindObjectOfType<SkillDatabase>();
        cController = FindObjectOfType<CharacterController>();
    }

    void Update()
    {
        if (cController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UseQuickSlotSkill(0);
            }

            if (skillIndicator.straightIndicator.activeSelf && Input.GetMouseButtonDown(0))
            {
                skillIndicator.straightIndicator.SetActive(false);
                skillDB.UseSkill(player.skillDic[0], gameObject); // ���߿� ��ų ���Կ� �ִ� ��ų�� �����ؾ���.
            }
        }
    }

    void UseQuickSlotSkill(int _slotIndex) // ���߿� ��ų���������� �ȱ�.
    {
        // z Ű�� �ִ� ��ų�� ������ �޾Ƽ� �ε������͸� ų�� ����.
        // �ε������͸� ų �ʿ䰡 ������ �ٷ� UseSkill �ߵ�
        //if (quickSkillSlot[0].skill.SkillType == Skill.SkillType.NoneTarget)
        //{
        //    skillIndicator.straightIndicator.SetActive(true);
        //}
        //else
        //{
        //    skillDB.UseSkill(�����Կ� �ִ� ��ų �ְ�, gameObject, target);
        //}
        #region �׽�Ʈ �ڵ�
        skillIndicator.straightIndicator.SetActive(true);
        #endregion
    }
}