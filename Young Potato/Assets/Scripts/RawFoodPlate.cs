using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawFoodPlate : Container
{




    // Start is called before the first frame update
    void Start()
    {
        player.DishBudget = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        Food food = other.transform.root.GetComponent<Food>();
      

        if(food)
        {
            if(!food.InContainer)
            {
                player.IngredientBudget -= food.FoodCost;
                food.InContainer = true;


            }
        }
    }
}
