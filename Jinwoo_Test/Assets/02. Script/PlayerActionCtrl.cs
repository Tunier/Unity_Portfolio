using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject wayPointUI;

    public List<GameObject> wayPoints = new List<GameObject>();
    public List<GameObject> merchants = new List<GameObject>();

    public CharacterController playerCtrl;
    float recognitionRange; //��ȣ�ۿ� �Ÿ� (��ȭ,��������Ʈ)
    void Awake()
    {
        recognitionRange = 2.5f;
    }
    private void Start()
    {
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WAYPOINT"));
        merchants.AddRange(GameObject.FindGameObjectsWithTag("MERCHANT"));
        playerCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        TryAction();
        CheckDistance();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (UIManager.instance.hotKeyGuid.activeSelf && wayPoints.Contains(UIManager.instance.hotKeyGuidTarget))
            {
                wayPointUI.SetActive(true);
            }
            else
            {
                wayPointUI.SetActive(false);
            }
            //else if(UIManager.instance.hotKeyGuid.activeSelf && merchants.Contains(UIManager.instance.hotKeyGuidTarget))
            //{
                //����uiŰ�� �κ��丮 Ű��
            //}
        }
    }

    private void CheckDistance()
    {
        foreach (GameObject _wayPoint in wayPoints) //��������Ʈ �ۿ� Ű Ȱ��ȭ
        {
            if (Vector3.Distance(transform.position, _wayPoint.transform.position) <= recognitionRange)
            {
                if (!wayPointUI.activeSelf)
                {
                    UIManager.instance.hotKeyGuid.SetActive(true);
                    UIManager.instance.hotKeyGuidTarget = _wayPoint;
                    return;
                }
            }
            else
            {
                UIManager.instance.hotKeyGuid.SetActive(false);
                //wayPointUI.SetActive(false);
            }
        }
        foreach (GameObject _merchant in merchants) //���� or NPC ��ȣ�ۿ� Ű Ȱ��ȭ
        {
            if (Vector3.Distance(transform.position, _merchant.transform.position) <= recognitionRange)
            {
                UIManager.instance.hotKeyGuid.SetActive(true);
                UIManager.instance.hotKeyGuidTarget = _merchant;
                return;
            }
            else
            {
                UIManager.instance.hotKeyGuid.SetActive(false);
            }
        }
    }

    
}
