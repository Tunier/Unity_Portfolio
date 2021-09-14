using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingletone<UIManager>
{
    public PlayerInfo player;

    public GameObject go_DamageText;

    public Canvas BackCanvas;

    public GameObject hotKeyGuid;
    public GameObject hotKeyGuidTarget;
    public Image fadeImg;
    public ButtonCtrl buttonCtrl;

    public List<GameObject> wayPoints = new List<GameObject>();
    public List<GameObject> merchants = new List<GameObject>();

    [SerializeField]
    Text explainTxt;
    [SerializeField]
    GameObject wayPointUI;

    float recognitionRange = 2.5f;
    private void Awake()
    {
        hotKeyGuid.SetActive(false);
    }

    private void Start()
    {
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WayPoint"));
        merchants.AddRange(GameObject.FindGameObjectsWithTag("Merchant"));
    }

    private void Update()
    {
        ShowExplainTxt();
        CheckDistance();
        ExitDistance();
    }

    public void ShowDamageText(float _Damage, bool _critical = false)
    {
        var go_damageText = Instantiate(go_DamageText, BackCanvas.transform);

        DamageTextUI DamageText = go_damageText.GetComponent<DamageTextUI>();

        DamageText.mob = player.targetMonster;

        DamageText.SetDamageText(_Damage);

        if (_critical)
            DamageText.SetTextColor(Color.red);
    }

    public void ShowExplainTxt()
    {
        if (hotKeyGuid.activeSelf)
        {
            if (hotKeyGuidTarget.CompareTag("WayPoint"))
            {
                explainTxt.text = "�۵���Ű��";
            }
            else if (hotKeyGuidTarget.CompareTag("MerChant")) //�±װ� �������,�����̸� �ٸ��� ���ڰ� ������
            {
                explainTxt.text = "��ȭ�ϱ�";
            }
        }
    }

    private void CheckDistance()
    {
        foreach (GameObject _wayPoint in wayPoints) //��������Ʈ �ۿ� Ű Ȱ��ȭ
        {
            if (Vector3.Distance(player.transform.position, _wayPoint.transform.position) <= recognitionRange)
            {
                if (!wayPointUI.activeSelf)
                {
                    hotKeyGuid.SetActive(true);
                    hotKeyGuidTarget = _wayPoint;
                    return;
                }
            }
            else
            {
                hotKeyGuid.SetActive(false);
            }
        }

        foreach (GameObject _merchant in merchants) //���� or NPC ��ȣ�ۿ� Ű Ȱ��ȭ
        {
            if (Vector3.Distance(player.transform.position, _merchant.transform.position) <= recognitionRange)
            {
                hotKeyGuid.SetActive(true);
                hotKeyGuidTarget = _merchant;
                return;
            }
            else
            {
                hotKeyGuid.SetActive(false);

            }
        }
    }

    /// <summary>
    /// �Ÿ��� �־����� UI������
    /// </summary>
    public void ExitDistance()
    {
        if (wayPointUI.activeSelf)
        {
            if (Vector3.Distance(player.transform.position, hotKeyGuidTarget.transform.position) > recognitionRange
                || hotKeyGuidTarget == null)
            {
                wayPointUI.SetActive(false);
            }
        }
    }

    /// <summary>
    /// fadein fadeout �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeCoroutine(float _darkTime, int _waynumber)
    {
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.03f;
            yield return new WaitForSeconds(0.03f);
            fadeImg.color = new Color(0, 0, 0, fadeCount);
        }
        buttonCtrl.moveWaypoint(_waynumber);
        yield return new WaitForSeconds(_darkTime);
        while (fadeCount > 0)
        {
            fadeCount -= 0.03f;
            yield return new WaitForSeconds(0.03f);
            fadeImg.color = new Color(0, 0, 0, fadeCount);
        }
    }

}