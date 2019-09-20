using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackTutorial : Tutorial
{
    public TutorialManager tutorialManager;
    private UIManager uiManager;

    public override void CheckIfHappening()
    {
      
     
            if (Input.GetKey(KeyCode.Space)) 
            {
                TutorialManager.Instance.CompletedAllTutorials();
            }
         
    }
}
