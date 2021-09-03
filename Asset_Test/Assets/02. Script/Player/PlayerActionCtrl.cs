using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCtrl : MonoBehaviour
{
    PlayerInfo player;
    SkillDatabase skillDB;
    Tooltip tooltip;

    public RectTransform inventoryRect;
    public RectTransform skillTreeRect;

    [SerializeField]
    Player_SkillIndicator skillIndicator;

    [SerializeField]
    GameObject inventoryUI;
    [SerializeField]
    GameObject skilltreeUI;
    [SerializeField]
    GameObject statsUI;

    public GameObject QuickSkillSlotParents;

    CharacterController cController;
    Animator ani;

    public List<SkillSlot> skillSlot = new List<SkillSlot>();

    SkillSlot readySkillSlot = null;
    Skill readySkill = null;

    [HideInInspector]
    public bool isWhirlwind = false;

    readonly int hashWhirlwind = Animator.StringToHash("IsWhirlwind");

    void Awake()
    {
        player = FindObjectOfType<PlayerInfo>();
        skillDB = FindObjectOfType<SkillDatabase>();
        tooltip = FindObjectOfType<Tooltip>();

        cController = FindObjectOfType<CharacterController>();
        ani = GetComponent<Animator>();

        skillSlot.AddRange(QuickSkillSlotParents.GetComponentsInChildren<SkillSlot>());
    }

    void Update()
    {
        if (cController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UseQuickSlotSkill(0);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                UseQuickSlotSkill(1);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                UseQuickSlotSkill(2);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                UseQuickSlotSkill(3);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                UseQuickSlotSkill(4);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                UseQuickSlotSkill(5);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                UseQuickSlotSkill(6);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                UseQuickSlotSkill(7);
            }
            else if (skillIndicator.straightIndicator.activeSelf && Input.GetMouseButtonDown(0))
            {
                skillIndicator.straightIndicator.SetActive(false);
                skillDB.UseSkill(readySkill, gameObject, null, readySkillSlot);
                readySkillSlot = null;
                readySkill = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            if (!inventoryUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, Input.mousePosition))
            {
                tooltip.HideTooltip();
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            skilltreeUI.SetActive(!skilltreeUI.activeSelf);
            if (statsUI.activeSelf)
                statsUI.SetActive(false);

            if (!skilltreeUI.activeSelf && RectTransformUtility.RectangleContainsScreenPoint(skillTreeRect, Input.mousePosition))
            {
                tooltip.HideTooltip();
            }
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            statsUI.SetActive(!statsUI.activeSelf);
            if (skilltreeUI.activeSelf)
                skilltreeUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryUI.SetActive(false);
            skilltreeUI.SetActive(false);
            statsUI.SetActive(false);
        }

        ani.SetBool(hashWhirlwind, isWhirlwind);
    }

    void UseQuickSlotSkill(int _slotIndex) // ���߿� ��ų���������� �ȱ�.
    {
        Skill _skill = skillSlot[_slotIndex].skill;
        SkillSlot _skillSlot = skillSlot[_slotIndex];
        if (_skillSlot.haveSkill)
        {
            if (_skillSlot.curCooltime <= 0)
            {
                switch (_skill.CostType)
                {
                    case 0:
                        // z Ű�� �ִ� ��ų�� ������ �޾Ƽ� �ε������͸� ų�� ����.
                        // �ε������͸� ų �ʿ䰡 ������ �ٷ� useskill �ߵ�
                        if (_skill.Type == 1)
                        {
                            skillIndicator.straightIndicator.SetActive(true);
                            readySkillSlot = _skillSlot;
                            readySkill = _skillSlot.skill;
                        }
                        else
                        {
                            SkillDatabase.instance.UseSkill(_skill, gameObject);
                        }
                        break;

                    case 1:
                        if (player.curHp >= _skill.Cost)
                        {
                            if (_skill.Type == 1)
                            {
                                skillIndicator.straightIndicator.SetActive(true);
                                readySkillSlot = _skillSlot;
                                readySkill = _skillSlot.skill;
                            }
                            else
                            {
                                SkillDatabase.instance.UseSkill(_skill, gameObject, null, _skillSlot);
                            }
                        }
                        else
                        {
                            print("������ �����մϴ�.");
                        }
                        break;

                    case 2:
                        if (player.curMp >= _skill.Cost)
                        {
                            if (_skill.Type == 1)
                            {
                                skillIndicator.straightIndicator.SetActive(true);
                                readySkillSlot = _skillSlot;
                                readySkill = _skillSlot.skill;
                            }
                            else
                            {
                                SkillDatabase.instance.UseSkill(_skill, gameObject, null, _skillSlot);
                            }
                        }
                        else
                        {
                            print("������ �����մϴ�.");
                        }
                        break;
                }
            }
            else
            {
                print("��ų�� ��Ÿ���Դϴ�.");
            }
            #region �׽�Ʈ �ڵ�
            //skillIndicator.straightIndicator.SetActive(true);
            #endregion
        }
    }
}
