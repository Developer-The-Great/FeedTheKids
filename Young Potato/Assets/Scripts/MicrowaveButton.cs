using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveButton : MonoBehaviour
{
    
    public BoxCollider heatingThing;

    public float cookTime;
    public float MaxCookTime;

    private MicrowaveDoor door;
  
    void Update()
    {
        cookTime -= Time.deltaTime;

        if(cookTime < 0)
        {
            cookTime = 0;
            heatingThing.enabled = false;
        }
    }

    public void addTime()
    {
        if(door.isOpen)
        {
            cookTime = MaxCookTime;
            heatingThing.enabled = true;
        }
        
    }

    public float GetMicrowaveProgress()
    {
        return cookTime / MaxCookTime;
    }
}
