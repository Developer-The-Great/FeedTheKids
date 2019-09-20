using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTutorial : Tutorial
{
    public override void CheckIfHappening()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
            TutorialManager.Instance.CompletedTutorial();

        }

    }

}
