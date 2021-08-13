using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject wayPointUI;

    void Awake()
    {
    }
    private void Start()
    {
    }

    void Update()
    {
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (UIManager.instance.hotKeyGuid.activeSelf)
            {
                if (UIManager.instance.wayPoints.Contains(UIManager.instance.hotKeyGuidTarget))
                {
                    wayPointUI.SetActive(true);
                }
            }
            else
            {
                wayPointUI.SetActive(false);
            }
            //else if(UIManager.instance.hotKeyGuid.activeSelf && merchants.Contains(UIManager.instance.hotKeyGuidTarget))
            //{
                //상점ui키고 인벤토리 키기
            //}
        }
    }

    

    
}
