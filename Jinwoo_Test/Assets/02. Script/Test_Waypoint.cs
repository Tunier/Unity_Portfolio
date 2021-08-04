using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Waypoint : MonoBehaviour
{
    GameObject player;
    public GameObject waypoint1;
    [SerializeField]
    GameObject button;

    Vector3 waypoint1v = new Vector3(-33f, 1f, 11f);

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position,player.transform.position)<= 3.0f)
        {
            button.SetActive(true);
        }
    }
   
    


    public void movePoint()
    {
        player.transform.position = waypoint1v;
    }
}
