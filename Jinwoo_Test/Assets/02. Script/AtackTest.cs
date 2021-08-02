using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtackTest : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<StatusController>().DecreaseHp(10);
        }
    }
}
