using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
 
    public Vector3 offset;

    private Grabber grabber;
    private Animator animation;

    private Quaternion targetRotation;

    private Vector3 initialOffset;
    private Vector3 mousePosition;
    // Start is called before the first frame update

    private void Awake()
    {
        animation = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        initialOffset = offset;
        grabber = transform.root.GetComponent<Grabber>();
    }

    // Update is called once per frame
   

    public void SetHandPosition(Vector3 mouseWorldPosition)
    {
        mousePosition = mouseWorldPosition;
        gameObject.transform.position = mouseWorldPosition + offset;
    }

    public void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.05f);

        
        
    }

    public void LateUpdate()
    {
        if (grabber.grabbedObject is Knife knife && Input.GetMouseButton(0))
        {
            Debug.Log("y lock");
            float y = knife.transform.position.y - mousePosition.y;
            float z = knife.transform.position.z - mousePosition.z;
            offset = new Vector3(offset.x, y, z-0.2f);
        }
        else
        {
            offset = initialOffset;
        }
    }

    public void DisableAll()
    {
        animation.SetBool("is_hovering", false);
        animation.SetBool("is_grabbing_knife", false);
        animation.SetBool("is_grabbing_food", false);
        targetRotation = Quaternion.identity;
    }


    public void SetHovering(Vector3 lookAt)
    {
        DisableAll();
        animation.SetBool("is_hovering", true);
        Vector3 forward = lookAt - transform.position;
        Vector3 right = Vector3.right;
        targetRotation = Quaternion.LookRotation(forward, Vector3.Cross(forward, right));

    }

    public void SetGrabbingKnife()
    {

        DisableAll();
        animation.SetBool("is_grabbing_knife", true);
        
    }

    public void SetGrabbingFood()
    {
        DisableAll();
        animation.SetBool("is_grabbing_food", true);
    }
}
