using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance = null;

    [Header("오브젝트 풀 정보")]
    public GameObject arrowPrefab;
    public int maxPool = 10;
    public List<GameObject> arrowPool = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        CreatePooling();
    }

    public void CreatePooling()
    {
        GameObject arrowPools = new GameObject("ArrowPools");

        for(int i = 0; i< maxPool; i++)
        {
            var obj = Instantiate<GameObject>(arrowPrefab, arrowPools.transform);
            obj.name = "Arrow_" + i.ToString("00");
            obj.SetActive(false);
            arrowPool.Add(obj);
        }
    }

    public GameObject GetArrow()
    {
        for(int i = 0; i < arrowPool.Count; i++)
        {
            if(arrowPool[i].activeSelf == false)
            {
                return arrowPool[i];
            }
        }
        return null;
    }


}






