using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTutorial : Tutorial
{
    public List<string> Keys = new List<string>();

    public Grabbable grabbedObject { get; set; }
    public Knife knife;

    private bool IsGrabbing;

    public override void CheckIfHappening()
    {
        if (IsGrabbing && grabbedObject == knife)
        {
            TutorialManager.Instance.CompletedTutorial();

        }
    }


}
