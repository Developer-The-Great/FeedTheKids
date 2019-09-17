using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawFoodPlate : Container
{

    [SerializeField] int foodCount;
    [SerializeField] int LastFoodCount;

    


    // Start is called before the first frame update
    void Start()
    {
        player.DishBudget = 0;
    }

    private void FixedUpdate()
    {
        foodCount = 0;
        foodInTray.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        Food food = CheckDuplicate( other.transform.root.GetComponent<Food>());
      

        if(food)
        {
            foodCount++;
            foodInTray.Add(food);
            if (!food.InContainer)
            {
                player.IngredientBudget -= food.FoodCost;
                food.InContainer = true;
                

            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Food food = other.transform.root.GetComponent<Food>();

        if (food)
        {
            foodCount++;
        }

    }
    public void Update()
    {
        
    }
}
