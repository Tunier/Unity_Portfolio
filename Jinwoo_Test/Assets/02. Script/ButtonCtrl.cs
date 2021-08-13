using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{
    GameObject player;

    public CharacterController playerCtrl;
    public List<GameObject> wayPoints = new List<GameObject>();

    float recognitionRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WAYPOINT"));
        recognitionRange = 2.5f;
    }

    public void OnClickHuman()
    {
        StartFade(0);
        playerCtrl.enabled = false;
    }

    public void OnClickElven()
    {
        StartFade(1);
        playerCtrl.enabled = false;
    }

    public void OnClickGoblin()
    {
        StartFade(2);
        playerCtrl.enabled = false;
    }

    public void OnClickUndead()
    {
        StartFade(3);
        playerCtrl.enabled = false;
    }

    public void OnClickExit()
    {
        GameManager.instance.wayPointUI.SetActive(false);
    }

    public void moveWaypoint(int _waynumber)
    {
        //�÷��̾ ���ִ� ��������Ʈ�� �̵��Ϸ��� ��������Ʈ �Ÿ��� �����̻� �־�� �۵�
        //�÷��̾ ���ִ� ��������Ʈ�� ������ ���� ����Ʈ��� �ν��ϵ���
        if (Vector3.Distance(player.transform.position, wayPoints[_waynumber].transform.position) > recognitionRange)
        {
            player.transform.position = wayPoints[_waynumber].transform.position;
            playerCtrl.enabled = true;
        }
        
    }

    public void StartFade(int _waynumber)
    {
        if (Vector3.Distance(player.transform.position, wayPoints[_waynumber].transform.position) > recognitionRange)
        {
            StartCoroutine(UIManager.instance.FadeCoroutine(0.5f,_waynumber));
        }
    }
}
