using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrowaveTutorial : Tutorial
{
    private MicrowaveDoor microwaveDoor;

    public override void CheckIfHappening()
    {
        if (microwaveDoor.isOpen == true)
        {
            TutorialManager.Instance.CompletedTutorial();
        }
    }

}
