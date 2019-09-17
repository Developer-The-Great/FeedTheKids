using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tray : Container
{

    
    Destroyer foodDestroyer;

    public List<Food> foodInTray { private set; get; }

    private float totalNutrition;

    public float DishCost { get; private set; }

    public float showDistCost;

    public GameObject debugShowObj;

   

    public float Nutrition
    {
        get { return totalNutrition; }
        private set { totalNutrition = value; }
    }

    private void Start()
    {
        player.DishBudget = 0;
        foodInTray = new List<Food>();
        //foodDestroyer = GetComponent<Destroyer>();

        

    }

    public void FixedUpdate()
    {
        DishCost = 0;
        totalNutrition = 0;
        foodInTray.Clear();
    }

    private void OnTriggerStay(Collider other)
    {

        Food foodFound = CheckDuplicate(other.transform.root.GetComponent<Food>());
        
        if (foodFound)
        {
      
            foodInTray.Add(foodFound);
            addFoodCost(foodFound);
    
            
        }

    }

    public Food CheckDuplicate(Food foodToCheck)
    {
        

        foreach(Food food in foodInTray)
        {
            if(food == foodToCheck)
            {
                return null;
            }
        }

        return foodToCheck;
    }

    public void Update()
    {


        showDistCost = DishCost;



    }
    private void addFoodCost(Food food)
    {
        DishCost += food.FoodCost;
    }

 

    public void DestroyFood()
    {
        //foodDestroyer.DestroyFood();
        Destroyer dest = gameObject.AddComponent<Destroyer>();
        StartCoroutine(dest.StartDestroy());
    }


}
