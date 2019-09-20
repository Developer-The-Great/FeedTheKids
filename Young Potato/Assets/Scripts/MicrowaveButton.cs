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

    public SoundManager soundManager;

    private void Awake()
    {
        door = GameObject.FindGameObjectWithTag("MicrowaveDoor").GetComponent<MicrowaveDoor>();

        bar.SetActive(false);

        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();


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
            soundManager.MicrowaveCook.Stop();
           
        }
    }

    public void addTime()
    {
        if(!door.isOpen)
        {
            bar.SetActive(true);
            heatingThing.enabled = true;
            cookTime = MaxCookTime;
            soundManager.MicrowaveCook.Play();
        }
       
    }

    public float GetMicrowaveProgress()
    {
        return cookTime / MaxCookTime;
    }
}
