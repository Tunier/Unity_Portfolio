using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapButton : MonoBehaviour
{
    public MiniMapCam minimapCam;

    private void Start()
    {
        minimapCam = minimapCam.GetComponent<MiniMapCam>();
    }
    public void OnClickPlus()
    {
        if (minimapCam.offset.y > 15)
        {
            minimapCam.offset.y -= 5;
        }
    }
  
    public void OnClickMinus()
    {
        if (minimapCam.offset.y < 30)
        {
            minimapCam.offset.y += 5;
        }
    }
}
