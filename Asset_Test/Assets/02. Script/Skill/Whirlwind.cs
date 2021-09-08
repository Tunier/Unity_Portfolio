using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    GameObject Player;

    PlayerActionCtrl playerAC;

    Vector3 pos;

    private void OnEnable()
    {
        Player = GameObject.Find("Player");
        playerAC = FindObjectOfType<PlayerActionCtrl>();

        StartCoroutine(StopMotion());

        Destroy(gameObject, 5.2f);
    }
    void Start()
    {
    }

    void Update()
    {
        pos = new Vector3(Player.transform.position.x, 1.5f, Player.transform.position.z);

        transform.position = pos;
    }

    IEnumerator StopMotion()
    {
        yield return new WaitForSeconds(5f);

        playerAC.isWhirlwind = false;
    }
}
