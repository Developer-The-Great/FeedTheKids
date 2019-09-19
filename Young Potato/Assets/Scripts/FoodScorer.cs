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

        
        float multiplier = 1.0001f;
        

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

            if(student.preferedFood == food.FoodType)
            {
                gotRequestedFood = true;
            }

            if(food.GetReadiness() > ReadinessThreshold)
            {
                fullyCookedCount++;
            }

            if (food.FoodType != FoodType.Apple)
            {
                if (food.Coldness > 0.5f)
                {
                    halfColdCount++;
                }

                if (food.Coldness > 0.7f)
                {
                    halfColdCount++;
                    if (food.Coldness > 0.95f)
                    {
                        freezingFood = true;
                    }
                }



                if (food.GetReadiness() < 0.5F)
                {
                    rawFood = true;
                }
            }
        }
        Debug.Log("scoring begins");
        //do multiplier addition
        if (majorBurnCount > 0)
        {
            if(majorBurnCount == foodInTray.Count)
            {
                Debug.Log("AllHeavilyBurntPnl");
                multiplier += AllHeavilyBurntPnl;
            }
            else if(majorBurnCount < foodInTray.Count)
            {
                Debug.Log("OneHeavilyBurntPnl");
                multiplier += OneHeavilyBurntPnl;
            }
            else
            {
                if (minorBurnCount > 0)
                {
                    if (minorBurnCount == foodInTray.Count)
                    {
                        Debug.Log("AllFoodBitBurntPnl");
                        multiplier += AllFoodBitBurntPnl;
                    }
                    else
                    {
                        Debug.Log("OneFoodBurntPnl");
                        multiplier += OneFoodBurntPnl;

                    }
                }
            }
        }

        if(fastcook)
        {
            Debug.Log("FastCookMlp");
            multiplier += FastCookMlp;
        }


        if(rawFood)
        {
            Debug.Log("rawFoodPnl");
            multiplier += rawFoodPnl;
        }

        if (gotRequestedFood)
        {
            Debug.Log("RequestedFoodMlp");
            multiplier += RequestedFoodMlp;
        }


        if(halfColdCount == foodInTray.Count)
        {
            Debug.Log("AllcoldFoodNoRawPnl");
            multiplier += AllcoldFoodNoRawPnl;
        }
        else if(halfColdCount < foodInTray.Count && halfColdCount >0)
        {
            Debug.Log("OnecoldFoodNoRawPnl");
            multiplier += OnecoldFoodNoRawPnl;
        }

        if(freezingFood)
        {
            Debug.Log("FreezingFoodPnl");
            multiplier += FreezingFoodPnl;
        }

        float result = Mathf.Clamp(multiplier, 0, multiplier);

        Debug.Log("Result " + result);
        return result;

    }

}
