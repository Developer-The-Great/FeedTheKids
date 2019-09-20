using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTutorial : Tutorial
{
    public List<string> Keys = new List<string>();
    public Grabbable grabbable { get; set; }


    public void Awake()
    {
       // = GameObject.FindGameObjectWithTag("Food").GetComponent<Food>();

    }



    public override void CheckIfHappening()
    {
        Debug.Log("hi");
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("it works");
            TutorialManager.Instance.CompletedTutorial();
        }
    }
}
