using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapButton : MonoBehaviour
{
    public Camera minimapCamera;

    public void OnClickPlus()
    {
        if (minimapCamera.orthographicSize > 33)
        {
            minimapCamera.orthographicSize -= 6;
        }
    }
  
    public void OnClickMinus()
    {
        if (minimapCamera.orthographicSize < 60)
        {
            minimapCamera.orthographicSize += 6;
        }
    }
}
