using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Container : MonoBehaviour
{
    public List<Food> foodInTray { private set; get; }

    protected Player player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetPlayer();
        foodInTray = new List<Food>();
        Debug.Assert(player);
    }

    protected Player GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    //
    protected Food CheckDuplicate(Food foodToCheck)
    {
        if (!foodToCheck) { return null; }

        foreach (Food food in foodInTray)
        {
            if (food == foodToCheck)
            {
                return null;
            }
        }

        return foodToCheck;
    }

}
