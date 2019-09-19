using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStudentTutorial : Tutorial
{
    public List<string> Keys = new List<string>();

    public override void CheckIfHappening()
    {

        if (Input.GetMouseButtonUp(1))
        {
            TutorialManager.Instance.CompletedTutorial();
        }
    }
}
