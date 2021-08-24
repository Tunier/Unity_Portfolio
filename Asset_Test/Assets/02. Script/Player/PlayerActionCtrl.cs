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

    [SerializeField]
    GameObject inventoryUI;
    [SerializeField]
    GameObject skilltreeUI;

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
                skillDB.UseSkill(skillDB.AllSkillDic["0300000"], gameObject); // ���߿� ��ų ���Կ� �ִ� ��ų�� �����ؾ���.
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            skilltreeUI.SetActive(!skilltreeUI.activeSelf);
        }
    }

    void UseQuickSlotSkill(int _slotIndex) // ���߿� ��ų���������� �ȱ�.
    {
        // z Ű�� �ִ� ��ų�� ������ �޾Ƽ� �ε������͸� ų�� ����.
        // �ε������͸� ų �ʿ䰡 ������ �ٷ� UseSkill �ߵ�
        //if (quickSkillSlot[0].skill.type == Skill.SkillType.NoneTarget)
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
