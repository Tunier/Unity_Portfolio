using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{
    GameObject player;

    public CharacterController playerCtrl;
    public List<GameObject> wayPoints = new List<GameObject>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("WAYPOINT"));
        playerCtrl = player.GetComponent<CharacterController>();

    }

    public void OnClickHuman()
    {
        playerCtrl.enabled = false;
        player.transform.position = wayPoints[0].transform.position + Vector3.up*5f;
        playerCtrl.enabled = true;
    }
    public void OnClickElven()
    {
        playerCtrl.enabled = false;
        player.transform.position = wayPoints[1].transform.position + Vector3.up*5f;
        playerCtrl.enabled = true;
    }
    public void OnClickGoblin()
    {
        playerCtrl.enabled = false;
        player.transform.position = wayPoints[2].transform.position + Vector3.up*5f;
        playerCtrl.enabled = true;
    }
    public void OnClickUndead() 
    {
        playerCtrl.enabled = false;
        player.transform.position = wayPoints[3].transform.position + Vector3.up*5f;
        playerCtrl.enabled = true;
    }
    public void OnClickExit()
    {
        GameManager.instance.wayPointUI.SetActive(false);
    }
}
