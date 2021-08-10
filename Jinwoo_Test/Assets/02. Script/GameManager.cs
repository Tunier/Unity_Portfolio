using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject wayPointUI;

    public bool isPause;

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
    }
    void Start()
    {
        isPause = false;
    }

    void Update()
    {
        UIHotKey();
        IsPause();
    }
    /// <summary>
    /// �κ��丮, ����â, ������ ������ ��� ���� Ű���� �Է��� ó����.
    /// </summary>
    void UIHotKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ui�� ��� ���������� ����
            if (!wayPointUI.activeSelf) //&& �ٸ� ui �߰�
            {
                isPause = !isPause;
            }
            else //ui�� ��� ���̶� ���������� ����
            {
                wayPointUI.SetActive(false);
            }
        }
    }
    public void IsPause()
    {
        if (isPause)
        {
            Time.timeScale = 0f;
            //����â ��Ƽ�� true
        }
        else
        {
            Time.timeScale = 1f;
            //����â ��Ƽ�� false;
        }
    }

    
    
}
