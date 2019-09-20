using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTutorial : Tutorial
{
    public override void CheckIfHappening()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TutorialManager.Instance.CompletedTutorial();

        }
    }
}
