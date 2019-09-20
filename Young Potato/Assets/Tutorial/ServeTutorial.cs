using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeTutorial : Tutorial
{
    public override void CheckIfHappening()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
            TutorialManager.Instance.CompletedAllTutorials();

        }
    }
}
