using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTutorial : Tutorial
{
    public List<string> Keys = new List<string>();

    public Grabbable grabbedObject { get; set; }
    public Knife knife;

    public override void CheckIfHappening()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TutorialManager.Instance.CompletedTutorial();

        }
    }


}
