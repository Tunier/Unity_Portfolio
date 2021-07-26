using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    float h;
    [SerializeField]
    float v;

    Vector3 movement;

    float moveSpeed = 3f;

    Rigidbody rb;
    NavMeshAgent nav;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        movement = new Vector3(h, 0, v) * moveSpeed;

        rb.velocity = movement;
    }
}
