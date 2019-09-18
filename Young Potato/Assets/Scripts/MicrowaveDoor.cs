using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveDoor : MonoBehaviour
{
    Quaternion closedRotation;
    Quaternion openRotation;

    public float angle;
    public GameObject colliderObj;

    public bool isOpen { private set; get; }

    // Start is called before the first frame update
    void Start()
    {
        Quaternion currentRotaion = transform.rotation;
        closedRotation = transform.rotation;
        openRotation = currentRotaion * Quaternion.AngleAxis(angle, Vector3.up);
    }

    private void Update()
    {
        if(isOpen)
        {
            colliderObj.GetComponent<BoxCollider>().enabled = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation,0.05f);
        }
        else
        {
            colliderObj.GetComponent<BoxCollider>().enabled = true;
            transform.rotation = Quaternion.Slerp(transform.rotation,closedRotation, 0.05f);
        }
        
    }
    void Open()
    {
        isOpen = true;
        
    }
    void Close()
    {
        isOpen = false;
        
    }

}
