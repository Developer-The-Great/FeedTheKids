using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatisfactionState
{
    Dislike,
    Ok,
    Like
}

public class UIManager : MonoBehaviour
{
    Player player;
    StudentManager studentManager;

    private Text ingredientText;
    private Text dishText;
    private Text budgetText;
    private Text nutritionText;
    private Text served;
    private Text tastiness;
    private Text cost;
    private Text studentIndexText;
    private Text moneyEarnedText;
    private Text starvingText;
    private Text requestText;


    private Text finishText;
    private Text likedText;
    private Text dislikeText;
    private Text okText;
    private Text moneyText;

    private Vector2 centerOffset;
    private Image centerCircle;
    private Image loadingCircle;
    private Image burntnessCircle;
    private Image endScreen;

    private CanvasScaler scaler;

    private GameObject bManager;
    [SerializeField] private Transform[] racks;
    [SerializeField] private GameObject[] bBills1;
    [SerializeField] private GameObject[] bBills2;
    [SerializeField] private GameObject[] bBills3;
    [SerializeField] private GameObject[] tBills1;
    [SerializeField] private GameObject[] tBills2;
    [SerializeField] private GameObject[] tBills3;

    private GameObject earnings;
    private GameObject buttons;
   

    private void Awake()
    {
        studentManager = GameObject.FindGameObjectWithTag("studentManager").GetComponent<StudentManager>();

        player = GetComponent<Player>();
        bManager = GameObject.FindGameObjectWithTag("Budgetmanager");
        GameObject[][] bRacks = {bBills1, bBills2, bBills3};
        GameObject[][] tRacks = {tBills1, tBills2, tBills3};
        for (int i = 0; i < 3; i++)
        {
            racks[i] = bManager.transform.GetChild(i);
            for (float j = 0; j < 20; j++)
            {
                if (j % 2 == 0)
                {
                    bRacks[i][(int)Mathf.Floor(j / 2)] = racks[i].GetChild((int)j).gameObject;
                }
                else
                {
                    tRacks[i][(int)Mathf.Floor(j / 2)] = racks[i].GetChild((int)j).gameObject;
                }
            }

        }
        
        

        moneyEarnedText = GameObject.FindGameObjectWithTag("moneyEarnedText").GetComponent<Text>();
        
        starvingText = GameObject.FindGameObjectWithTag("StarvingKidsText").GetComponent<Text>();

        studentIndexText = GameObject.FindGameObjectWithTag("studentIndexText").GetComponent<Text>();

        ingredientText = GameObject.FindGameObjectWithTag("ingredientText").GetComponent<Text>();

        served = GameObject.FindGameObjectWithTag("servingText").GetComponent<Text>();

        tastiness = GameObject.FindGameObjectWithTag("tastinessText").GetComponent<Text>();

        loadingCircle = GameObject.FindGameObjectWithTag("loadingCircle").GetComponent<Image>();

        burntnessCircle = GameObject.FindGameObjectWithTag("loadingBurntness").GetComponent<Image>();

        centerCircle = GameObject.FindGameObjectWithTag("centerCircle").GetComponent<Image>();

        scaler = GameObject.FindGameObjectWithTag("canvas").GetComponent<CanvasScaler>();

        endScreen = GameObject.FindGameObjectWithTag("endscreen").GetComponent<Image>();

        RectTransform rectTransform = loadingCircle.gameObject.GetComponent<RectTransform>();

        float width = (rectTransform.rect.width * scaler.scaleFactor) / 4;
        float height = (rectTransform.rect.height * scaler.scaleFactor) / 4;

        centerOffset = new Vector2(width, height);

        cost = GameObject.FindGameObjectWithTag("cost").GetComponent<Text>();
        cost.text = "";

        studentIndexText.text = "";
        UpdateMoneyEarnedText();

        finishText = GameObject.Find("Finish").GetComponent<Text>();
        likedText = GameObject.Find("likedText").GetComponent<Text>();
        dislikeText = GameObject.Find("dislikeText").GetComponent<Text>();
        okText = GameObject.Find("okText").GetComponent<Text>();
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        requestText = GameObject.Find("request").GetComponent<Text>();

        earnings = GameObject.FindGameObjectWithTag("Earnings");
        buttons = GameObject.FindGameObjectWithTag("Buttons");

    }

    private void Start()
    {
        served.gameObject.SetActive(false);
        tastiness.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);


        finishText.gameObject.SetActive(false);
        likedText.gameObject.SetActive(false);
        dislikeText.gameObject.SetActive(false);
        okText.gameObject.SetActive(false);
        moneyText.gameObject.SetActive(false);
        starvingText.gameObject.SetActive(false);
       
    }

    public void UpdateIngredientText()
    {
        ingredientText.text = "Ingredient Cost: " + player.IngredientBudget;
    }

    public void UpdateDishText()
    {
        GameObject[][] bRacks = {bBills1, bBills2, bBills3};
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                bRacks[i][j].GetComponent<Image>().fillAmount = 0;
            }
            if (player.tray[i].DishCost % 1 == 0)
            {
                for (int j = 0; j < player.tray[i].DishCost; j++)
                {
                    bRacks[i][j].GetComponent<Image>().fillAmount = 1;
                }
            }
            else
            {
                for (int j = 0; j < player.tray[i].DishCost-1; i++)
                {
                    bRacks[i][j].GetComponent<Image>().fillAmount = 1;
                    if (j == player.tray[i].DishCost-1)
                    {
                        bRacks[i][j+1].GetComponent<Image>().fillAmount = player.tray[i].DishCost % 1;
                    }
                }
                
            }
        }

    }

    public void UpdateBudgetText()
    {
        GameObject[][] tRacks = {tBills1, tBills2, tBills3};
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tRacks[i][j].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0);
            }
            if (player.currentlyServing[i] == null) continue;
            for (int j = 0; j < player.currentlyServing[i].StudentBudget; j++)
            {
                tRacks[i][j].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
            }
        }

        
    }

    public void UpdateIndexText()
    {
        studentIndexText.text = "Student #" + (player.currentStudentIndex+1);
    }

    public void UpdateLikenessText(StatisfactionState likeness)
    {
        switch (likeness)
        {
            case StatisfactionState.Dislike:
                tastiness.text = "The student disliked that!";
                break;
            case StatisfactionState.Like:
                tastiness.text = "The student liked that!";
                break;
            case StatisfactionState.Ok:
                tastiness.text = "The student was ok with it...";
                break;
        }

           

    }

    public void UpdateMoneyEarnedText()
    {
        float whole = Mathf.Round(studentManager.moneyEarned);
        float deci = Mathf.Round((studentManager.moneyEarned - whole)*100)/100;
        float earned = whole + deci;
        moneyEarnedText.text = "$" + earned;
    }

    public void UpdateRequestText(FoodType foodType)
    {
        switch(foodType)
        {
            case FoodType.Apple:
                requestText.text = "Student Request: Apple" ;
                break;
            case FoodType.Chicken:
                requestText.text = "Student Request: Chicken";
                break;
            case FoodType.Potato:
                requestText.text = "Student Request: Potato";
                break;
            case FoodType.Rice:
                requestText.text = "Student Request: Rice";
                break;
            case FoodType.None:
                requestText.text = "Student Request: None";
                break;

        }
        



    }


    public void DisplayFoodInformation(Food food)
    {
        //
        //
        Vector2 mousePosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width, Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);

        loadingCircle.gameObject.GetComponent<RectTransform>().anchoredPosition = mousePosition;
        loadingCircle.fillAmount = food.GetReadiness();
        loadingCircle.gameObject.SetActive(true);


        burntnessCircle.gameObject.GetComponent<RectTransform>().anchoredPosition = mousePosition;
        burntnessCircle.fillAmount = food.Burntness;
        burntnessCircle.gameObject.SetActive(true);

        centerCircle.gameObject.GetComponent<RectTransform>().anchoredPosition = mousePosition + centerOffset;
        centerCircle.gameObject.SetActive(true);

        cost.gameObject.GetComponent<RectTransform>().anchoredPosition = mousePosition + new Vector2(0, 12);
        cost.text = "$" + food.FoodCost;
        cost.gameObject.SetActive(true);


    }
    public void DeactivateFoodInformation()
    {
        loadingCircle.gameObject.SetActive(false);
        burntnessCircle.gameObject.SetActive(false);
        centerCircle.gameObject.SetActive(false);
        cost.gameObject.SetActive(false);

    }

   
    // Update is called once per frame
    void Update()
    {
        if (studentManager.CheckWin())
        {
            ingredientText.gameObject.SetActive(false);
            bManager.gameObject.SetActive(false);
            earnings.gameObject.SetActive(false);
            buttons.gameObject.SetActive(false);

            served.gameObject.SetActive(false);
            tastiness.gameObject.SetActive(false);
            cost.gameObject.SetActive(false);
            studentIndexText.gameObject.SetActive(false);
            moneyEarnedText.gameObject.SetActive(false);


            likedText.text = "Times food was liked " + studentManager.likeCount;
            dislikeText.text = "Times food was disliked " + studentManager.dislikeCount;
            okText.text = "Times food was ok " + studentManager.okCount;
            starvingText.text = studentManager.starvingCount + " Starving Children";
            if( studentManager.starvingCount > 0)
                starvingText.color = new Color(219.0f/255.0f, 79.0f/255.0f, 56.0f/255.0f, 1.0f);
            float whole = Mathf.Round(studentManager.moneyEarned);
            float deci = Mathf.Round((studentManager.moneyEarned - whole)*100)/100;
            float earned = whole + deci;
            moneyText.text = "Money Earned $" + earned;

            endScreen.gameObject.SetActive(true);
            finishText.gameObject.SetActive(true);
            likedText.gameObject.SetActive(true);
            dislikeText.gameObject.SetActive(true);
            okText.gameObject.SetActive(true);
            moneyText.gameObject.SetActive(true);
            starvingText.gameObject.SetActive(true);




        }

    }
    public IEnumerator displayServed(StatisfactionState likeness)
    {
        UpdateLikenessText(likeness);

        served.gameObject.SetActive(true);
        tastiness.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        served.gameObject.SetActive(false);
        tastiness.gameObject.SetActive(false);
    }

    public void next1()
    {
        player.fillPosition(0,false);
    }
    public void next2()
    {
        player.fillPosition(1,false);
    }
    public void next3()
    {
        player.fillPosition(2,false);
    }
}
