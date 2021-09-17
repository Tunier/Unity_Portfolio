using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapController : MonoBehaviour
{
    [SerializeField]
    RectTransform worldMapImgRect;

    private void Awake()
    {
        worldMapImgRect.localScale = new Vector3(0.6f, 0.6f);
    }

    private void Update()
    {
        ZoomFunc();
    }
    public void OnClickPlus() 
    {
        if (worldMapImgRect.localScale.x < 2)
        {
            worldMapImgRect.localScale += new Vector3(0.28f, 0.28f);
        }
    }
    
    public void OnClickMinus()
    {
        if (worldMapImgRect.localScale.x > 0.7f)
        {
            worldMapImgRect.localScale -= new Vector3(0.28f, 0.28f);
        }
    }

    public void ZoomFunc()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        if (worldMapImgRect.localScale.x >0.7f && scroll < 0)
        {
            worldMapImgRect.localScale -= new Vector3(0.28f, 0.28f);
        }
        else if(worldMapImgRect.localScale.x <2 && scroll>0) 
        {
            worldMapImgRect.localScale += new Vector3(0.28f,0.28f);
        }
        


    }
}
