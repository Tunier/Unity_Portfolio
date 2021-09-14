using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : MonoBehaviour
{
    GameObject Player;
    GameObject PlayerBody;

    Vector3 pos;

    private void OnEnable()
    {
        Player = GameObject.Find("Player");
        PlayerBody = GameObject.Find("Hips_jnt");
    }
    void Start()
    {
    }

    void Update()
    {
        pos = new Vector3(Player.transform.position.x, 1.5f, Player.transform.position.z);

        transform.position = pos;
    }
}
