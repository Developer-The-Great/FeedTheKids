using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{

    [SerializeField] Vector3 direction;
    [SerializeField]float addForce;

   


    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.transform.root.GetComponent<Rigidbody>();

        if(rb)
        {
            rb.AddForce(direction * addForce);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("check exit");
    }
}
