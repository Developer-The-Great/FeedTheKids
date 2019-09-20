using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackTutorial : Tutorial
{
    public TutorialManager tutorialManager;

    public override void CheckIfHappening()
    {
            if (Input.GetKeyUp(KeyCode.Space)) 
            {
                TutorialManager.Instance.CompletedAllTutorials();
            }
         
    }
}
