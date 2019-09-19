using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveButton : MonoBehaviour
{
    
    public BoxCollider heatingThing;

    public float cookTime;
    public float MaxCookTime;

    private MicrowaveDoor door;

    public GameObject bar;
    public GameObject meter;
    private void Awake()
    {
        door = GameObject.FindGameObjectWithTag("MicrowaveDoor").GetComponent<MicrowaveDoor>();

        Transform BothBars =   transform.root.Find("BothBars");
        bar.SetActive(false);
   

        //meter = bar.transform.Find("MicrowaveBar").gameObject;
    }
    void Update()
    {
        cookTime -= Time.deltaTime;
        meter.transform.localScale = new Vector3(GetMicrowaveProgress(), 1, 1);
        if(cookTime < 0)
        {
            bar.SetActive(false);
            cookTime = 0;
            heatingThing.enabled = false;
           
        }
    }

    public void addTime()
    {
        if(!door.isOpen)
        {
            Debug.Log("cook");
            bar.SetActive(true);
            cookTime = MaxCookTime;
            heatingThing.enabled = true;
        }
       
    }

    public float GetMicrowaveProgress()
    {
        return cookTime / MaxCookTime;
    }
}
