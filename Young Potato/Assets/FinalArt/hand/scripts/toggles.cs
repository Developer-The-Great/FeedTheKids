using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggles : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private bool hover = false;

    private bool knife = false;

    private bool food = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hover()
    {
        if (!hover)
        {
            animator.SetBool("is_hovering", true);
            hover = true;
        }
        else
        {
            animator.SetBool("is_hovering", false);
            hover = false;
        }
    }

    public void Knife()
    {
        if (!knife)
        {
            animator.SetBool("is_grabbing_knife", true);
            knife = true;
        }
        else
        {
            animator.SetBool("is_grabbing_knife", false);
            knife = false;
        }
    }

    public void Food()
    {
        if (!food)
        {
            animator.SetBool("is_grabbing_food", true);
            food = true;
        }
        else
        {
            animator.SetBool("is_grabbing_food", false);
            food = false;
        }
    }
}
