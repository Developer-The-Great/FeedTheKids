using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnTutorial : Tutorial
{

    Food food; 
    public override void CheckIfHappening()
    {
        if (food.Friedness >= 1.01f )
        {
            //Burnt Food 
            TutorialManager.Instance.CompletedTutorial();
        }
    }

}
