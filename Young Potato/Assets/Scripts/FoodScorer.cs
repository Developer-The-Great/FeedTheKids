using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScorer : MonoBehaviour
{
    [SerializeField]private float RequestedFoodMlp;
    [SerializeField] private float NoRequestedFoodMlp;

    [SerializeField] private float OneFoodBurntPnl;
    [SerializeField] private float AllFoodBitBurntPnl;
    [SerializeField] private float OneHeavilyBurntPnl;
    [SerializeField] private float AllHeavilyBurntPnl;


    [SerializeField] private float AllfullyCookedMlp;

    [SerializeField] private float FastCookMlp;

    [SerializeField] private float AllOptimalMlp;

    [SerializeField] private float rawFoodPnl;

    [SerializeField] private float OnecoldFoodNoRawPnl;

    [SerializeField] private float AllcoldFoodNoRawPnl;

    [SerializeField] private float FreezingFoodPnl;

    [SerializeField] [Range(0, 1)] private float BurntForgiveness = 0.1f;
    [SerializeField] [Range(0, 1)] private float minorBurnt = 0.2f;
    [SerializeField] [Range(0, 1)] private float ReadinessThreshold = 0.90f;

    public float GetFoodMultiplier(Student student,List<Food> foodInTray)
    {

        
        float multiplier = 1.01f;
        

        if(student.isBroke)
        {
            return multiplier;
        }

        int minorBurnCount = 0;

        int majorBurnCount = 0;

        bool gotRequestedFood = false;

        int fullyCookedCount = 0;

        int halfColdCount = 0;

        bool fastcook = student.GetWaitPercentage() < 0.5f;


        bool rawFood = false;

        bool freezingFood = false;
        

        //for each food in tray
        foreach(Food food in foodInTray)
        {

            if(food.Burntness > BurntForgiveness)
            {
                if(food.Burntness > minorBurnt)
                {
                    minorBurnCount++;
                }
                else
                {
                    majorBurnCount++;
                }
            }

            if(student.preferedFood == food.Type)
            {
                gotRequestedFood = true;
            }

            if(food.GetReadiness() > ReadinessThreshold)
            {
                fullyCookedCount++;
            }

            if(food.Coldness > 0.5f)
            {
                halfColdCount++;
            }

            if (food.Coldness > 0.5f)
            {
                halfColdCount++;
                if (food.Coldness > 0.9f)
                {
                    freezingFood = true;
                }
            }


            if(food.GetReadiness() < 0.5F)
            {
                rawFood = true;
            }

        }

        //do multiplier addition
        if(majorBurnCount > 0)
        {
            if(majorBurnCount == foodInTray.Count)
            {
                multiplier += AllHeavilyBurntPnl;
            }
            else if(majorBurnCount < foodInTray.Count)
            {
                multiplier += OneHeavilyBurntPnl;
            }
            else
            {
                if (minorBurnCount > 0)
                {
                    if (minorBurnCount == foodInTray.Count)
                    {
                        multiplier += AllFoodBitBurntPnl;
                    }
                    else
                    {
                        multiplier += OneFoodBurntPnl;

                    }
                }
            }
        }

        if(fastcook)
        {
            multiplier += FastCookMlp;
        }


        if(rawFood)
        {
            multiplier += rawFoodPnl;
        }

        if (gotRequestedFood)
        {
            multiplier += RequestedFoodMlp;
        }


        if(halfColdCount == foodInTray.Count)
        {
            multiplier += AllcoldFoodNoRawPnl;
        }
        else if(halfColdCount < foodInTray.Count && halfColdCount >0)
        {
            multiplier += OnecoldFoodNoRawPnl;
        }

        if(freezingFood)
        {
            multiplier += FreezingFoodPnl;
        }
        return Mathf.Clamp(multiplier, 0, multiplier); ;

    }

}
