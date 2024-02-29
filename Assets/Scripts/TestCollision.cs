using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision !");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger !");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 look = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);

        RaycastHit hit;
        if( Physics.Raycast(transform.position + Vector3.up, look, out hit, 10))
        {
            Debug.Log($"Raycast {hit.collider.gameObject.name}");
        }
    }
}
