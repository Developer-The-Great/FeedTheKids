using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tray : Container
{

    
    Destroyer foodDestroyer;

   


    public float DishCost { get; private set; }

    public float showDistCost;

    public GameObject debugShowObj;

   

   

    private void Start()
    {
        player.DishBudget = 0;
       
        //foodDestroyer = GetComponent<Destroyer>();

        

    }

    public void FixedUpdate()
    {
        DishCost = 0;
        
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
