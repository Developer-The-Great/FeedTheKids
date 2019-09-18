using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    public Vector3 offset;
    private Animator animation;
    // Start is called before the first frame update

    private void Awake()
    {
        animation = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SetHovering();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            SetGrabbingKnife();
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            SetGrabbingFood();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            disableAll();
        }

    }

    public void SetHandPosition(Vector3 mouseWorldPosition)
    {
        gameObject.transform.position = mouseWorldPosition + offset;
    }


    private void disableAll()
    {
        animation.SetBool("is_hovering", false);
        animation.SetBool("is_grabbing_knife", false);
        animation.SetBool("is_grabbing_food", false);
    }


    public void SetHovering()
    {
        disableAll();
        animation.SetBool("is_hovering", true);
    }

    public void SetGrabbingKnife()
    {
        disableAll();
        animation.SetBool("is_grabbing_knife", true);
    }

    public void SetGrabbingFood()
    {
        disableAll();
        animation.SetBool("is_grabbing_food", true);
    }
}
