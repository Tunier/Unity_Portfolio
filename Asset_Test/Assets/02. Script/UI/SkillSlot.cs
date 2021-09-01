using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Skill skill;
    public Image skillImage;
    public Text skillTreeLvText;
    public float curCooltime;
    public GameObject cooldownImage;
    public Text cooldownText;
    public bool haveSkill { get; private set; } = false;


    Tooltip tooltip;
    Skill_Tree_UI skillTreeUI;

    PlayerInfo player;

    private void Awake()
    {
        tooltip = FindObjectOfType<Tooltip>();
        player = FindObjectOfType<PlayerInfo>();
        skillTreeUI = FindObjectOfType<Skill_Tree_UI>();
    }

    private void Start()
    {
        skillTreeUI.skillOptionPanel.SetActive(false);
    }

    private void Update()
    {
        if (curCooltime > 0)
        {
            cooldownImage.SetActive(true);
            curCooltime -= Time.deltaTime;
            cooldownImage.GetComponent<Image>().fillAmount = curCooltime / skill.CoolTime;
            cooldownText.text = (Mathf.FloorToInt(curCooltime)).ToString();
        }
        else if (curCooltime < 0)
        {
            cooldownImage.SetActive(false);
            curCooltime = 0;
            cooldownText.text = "0";
        }

        if (skillTreeLvText != null)
            skillTreeLvText.text = player.player_Skill_Dic[skill.UIDCODE].ToString();
    }

    void SetColorAlpha(float alpha)
    {
        Color color = skillImage.color;
        color.a = alpha;
        skillImage.color = color;
    }

    /// <summary>
    /// 슬롯에 스킬을 추가하고, 스킬 이미지를 뜨게함.
    /// </summary>
    /// <param name="_item"></param>
    /// <param name="count"></param>
    public void AddSkill(Skill _skill)
    {
        skill = _skill;
        skillImage.sprite = Resources.Load<Sprite>(_skill.SkillImagePath);
        SetColorAlpha(1);
        haveSkill = true;
    }

    public void ClearSlot()
    {
        skill = null;
        skillImage.sprite = null;
        SetColorAlpha(0);
        haveSkill = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (gameObject.CompareTag("SkillTreeSlot"))
            {
                skillTreeUI.skillOptionPanel.transform.localPosition = new Vector3(transform.localPosition.x + 61.4f, transform.localPosition.y - 44, 0);
                skillTreeUI.skillOptionPanel.SetActive(true);
                skillTreeUI.curSlot = this;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (skill.Type != 0)
            {
                DragSlot.instance.dragSkillSlot = this;
                DragSlot.instance.DragSetImage(skillImage);
                DragSlot.instance.transform.position = eventData.position;
            }
            else
            {
                DragSlot.instance.SetColorAlpha(0);
                DragSlot.instance.dragSlot = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (haveSkill == true)
            DragSlot.instance.transform.position = eventData.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (haveSkill == true)
            tooltip.ShowTooltip(skill);

        DragSlot.instance.SetColorAlpha(0);
        DragSlot.instance.dragSlot = null;
    }

    /// <summary>
    /// 아이템 있으면 툴팁출력.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (haveSkill == true)
            tooltip.ShowTooltip(skill);
    }

    /// <summary>
    /// 툴팁 꺼지게함.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (gameObject.CompareTag("QuickSkillSlot"))
        {
            if (DragSlot.instance.dragSkillSlot != null)
                AddSkill(DragSlot.instance.dragSkillSlot.skill);
        }
    }
}
