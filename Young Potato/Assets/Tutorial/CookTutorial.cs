﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookTutorial : Tutorial
{
    public override void CheckIfHappening()
    {

        if (Input.GetKeyUp(KeyCode.Space))
        {
            TutorialManager.Instance.CompletedTutorial();

        }

    }
}
