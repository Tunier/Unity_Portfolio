using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapButton : MonoBehaviour
{
    public MiniMapCam minimapCam;
    [SerializeField]
    GameObject PlayerMark;

    private void Start()
    {
        minimapCam = minimapCam.GetComponent<MiniMapCam>();
    }
    public void OnClickPlus()
    {
        if (minimapCam.offset.y > 35)
        {
            minimapCam.offset.y -= 5;
            PlayerMark.transform.position = new Vector3(PlayerMark.transform.localPosition.x, PlayerMark.transform.localPosition.y - 5);
        }
    }
  
    public void OnClickMinus()
    {
        if (minimapCam.offset.y < 50)
        {
            minimapCam.offset.y += 5;
            PlayerMark.transform.position = new Vector3(PlayerMark.transform.localPosition.x, PlayerMark.transform.localPosition.y + 5);
        }
    }
}
