using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    
    public GameObject hotKeyGuid;
    public GameObject hotKeyGuidTarget;

    [SerializeField]
    private Text explainTxt;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        hotKeyGuid.SetActive(false);
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        ShowExplainTxt();
    }

    public void ShowExplainTxt()
    {
        if (hotKeyGuid.activeSelf)
        {
            if (hotKeyGuidTarget.CompareTag("WAYPOINT"))
            {
                explainTxt.text ="�۵���Ű��";
            }
            else if(hotKeyGuidTarget.CompareTag("MERCHANT")) //�±װ� �������,�����̸� �ٸ��� ���ڰ� ������
            {
                explainTxt.text = "��ȭ�ϱ�";
            }
        }
    }
}
