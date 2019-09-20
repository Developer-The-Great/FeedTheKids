using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tray : Container
{
    public float DishCost { get; private set; }

    public float showDistCost;

    public GameObject debugShowObj;

    private void Start()
    {
        
        player.DishBudget = 0;
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

    public void GiveFood(Student student)
    {
        foreach(Food food in foodInTray)
        {
            food.SetStickValues(student.trayInStudent);
        }
    }
   
}
