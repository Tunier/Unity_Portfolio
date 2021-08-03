using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot_Skill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Skill skill;
    public float coolTime;
    public float curCoolTime;
    public Image skillImage;
    
    [SerializeField]
    GameObject cooldownImage;
    [SerializeField]
    Text cooltimeTxt;
    [SerializeField]
    RectTransform quickSlot;
    [SerializeField]
    ToolTip_Skill toolTip;

    Image cooldown;
    void Awake()
    {
        cooldown = GetComponent<Image>();
    }

    void Update()
    {
        SetCoolDown();
    }
    //���� ���İ��� �޾Ƽ� ���� (0 or0.6)
    void SetColorAlpha(float alpha)
    {
        Color color = skillImage.color;
        color.a = alpha;
        skillImage.color = color;
    }
    /// <summary>
    /// ��ų ���Կ� ��ų�� �־��ְ�, �̹����� ��ų �̹����� �ٲ��ְ�, �ش� ��ų��Ÿ�Ӱ� ���� ��Ÿ���� �Ѱ���.
    /// </summary>
    /// <param name="_skill"></param>
    /// <param name="_curCooltime"></param>
    public void AddSkillicon(Skill _skill,float _curCooltime)
    {
        skill = _skill;
        skillImage.sprite = _skill.skillImage;
        coolTime = _skill.coolTime;
        curCoolTime = _curCooltime;

        //�ش� ��ų�� ��Ÿ�� �̶�� ��Ÿ�� �̹����� active ��Ű��
        //��Ÿ���� ǥ�� �ǰ� �ϴ� ��� �־����.

        SetColorAlpha(0.6f);
    }
    //���Կ� ��ų�� �ű�ų� ��ĭ���� ����ÿ�
    void ClearSlot()
    {
        skill = null;
        coolTime = 0f;
        curCoolTime = 0f;
        skillImage.sprite = null;
        SetColorAlpha(0);

        cooltimeTxt.text = "0";
        cooldownImage.SetActive(false);
    }
    void SetCoolDown()
    {
        if (curCoolTime >= 0f)
        {
            cooldownImage.SetActive(true);
            curCoolTime -= Time.deltaTime;
        }
        else if (curCoolTime < 0f)
        {
            cooldownImage.SetActive(false);
            curCoolTime = 0f;
        }
        cooltimeTxt.text = Mathf.Ceil(curCoolTime).ToString();
        cooldown.fillAmount = curCoolTime / coolTime;
    }
    //���콺 �巡�׸� ���������� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    //���콺 �巡�� ���� ���� ��� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
    }
    //��� �� ���𰡰� ���� �� ȣ�� *OnDrop�� OnEndDrag���� ���� ����ȴ�.
    public void OnDrop(PointerEventData eventData)
    {
    }
    //���콺 �巡�� �ϴ� ���� ������ �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
    }
    //���콺�����Ͱ� �浹�����ȿ� ��� �ö� ������ �̺�Ʈ
    public void OnPointerEnter(PointerEventData eventData)
    {
    }
    //���콺�� �����Ͱ� �浹���������� ���� �� ������ �̺�Ʈ
    public void OnPointerExit(PointerEventData eventData)
    {
    }

}
