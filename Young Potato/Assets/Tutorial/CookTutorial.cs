using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookTutorial : Tutorial
{
    private Food food;

    public void Start()
    {
        food = GameObject.FindGameObjectWithTag("Food").GetComponent<Food>();
    }
    public override void CheckIfHappening()
    {
      //  Debug.Log(food.Friedness);

        if (food.Friedness > 1.0f && food.Friedness < 1.5f)
        {
            TutorialManager.Instance.CompletedTutorial();
        }
        
    }
}
