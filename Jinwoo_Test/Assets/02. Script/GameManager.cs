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
    /// 인벤토리, 스텟창, 퀵슬롯 아이템 사용 등의 키보드 입력을 처리함.
    /// </summary>
    void UIHotKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ui가 모두 꺼져있으면 실행
            if (!wayPointUI.activeSelf) //&& 다른 ui 추가
            {
                isPause = !isPause;
            }
            else //ui가 어떠한 것이라도 꺼져있으면 실행
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
            //설정창 엑티브 true
        }
        else
        {
            Time.timeScale = 1f;
            //설정창 엑티브 false;
        }
    }

    
    
}
