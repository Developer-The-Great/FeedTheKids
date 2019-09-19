using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTutorial : Tutorial
{
    public List<string> Keys = new List<string>();
    
   // public Grabber grabbedObject;
    public Player player;

    public override void CheckIfHappening()
    {
       // GameObject.FindGameObjectsWithTag("Player");
       /*
        if (player.grabber.IsGrabbing == true)
        {

            TutorialManager.Instance.CompletedTutorial();
            player.grabber.IsGrabbing = false;
            
        }

       /*
            if (Input.inputString.Contains(Keys[i]))
            {
                Keys.RemoveAt(i);
                break;
            }

        if (Keys.Count == 0)
        {
            TutorialManager.Instance.CompletedTutorial();
        }
        */
    }
}
