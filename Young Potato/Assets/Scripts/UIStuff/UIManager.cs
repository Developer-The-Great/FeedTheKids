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
    private Text requestText;


    private Text finishText;
    private Text likedText;
    private Text dislikeText;
    private Text okText;
    private Text moneyText;

    private Vector2 centerOffset;
    private Image centerCircle;
    private Image loadingCircle;
    private Image endScreen;

    private CanvasScaler scaler;


   

    private void Awake()
    {
        studentManager = GameObject.FindGameObjectWithTag("studentManager").GetComponent<StudentManager>();

        player = GetComponent<Player>();
        dishText = GameObject.FindGameObjectWithTag("dishes").GetComponent<Text>();

        moneyEarnedText = GameObject.FindGameObjectWithTag("moneyEarnedText").GetComponent<Text>();

        studentIndexText = GameObject.FindGameObjectWithTag("studentIndexText").GetComponent<Text>();

        ingredientText = GameObject.FindGameObjectWithTag("ingredientText").GetComponent<Text>();

        budgetText = GameObject.FindGameObjectWithTag("budgetText").GetComponent<Text>();
        served = GameObject.FindGameObjectWithTag("servingText").GetComponent<Text>();

        tastiness = GameObject.FindGameObjectWithTag("tastinessText").GetComponent<Text>();

        loadingCircle = GameObject.FindGameObjectWithTag("loadingCircle").GetComponent<Image>();

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
       
    }

    public void UpdateIngredientText()
    {
        ingredientText.text = "Ingredient Cost: " + player.IngredientBudget;
    }

    public void UpdateDishText()
    {
        dishText.text = "Dish Cost: " + player.DishBudget;
        
    }

    public void UpdateBudgetText()
    {
        budgetText.text = "Student Budget: " + player.StudentBudget;
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
        moneyEarnedText.text = "Money earned: $" + studentManager.moneyEarned;
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

        centerCircle.gameObject.GetComponent<RectTransform>().anchoredPosition = mousePosition + centerOffset;
        centerCircle.gameObject.SetActive(true);

        cost.gameObject.GetComponent<RectTransform>().anchoredPosition = mousePosition + new Vector2(0, 5);
        cost.text = food.FoodCost + "$";
        cost.gameObject.SetActive(true);


    }
    public void DeactivateFoodInformation()
    {
        loadingCircle.gameObject.SetActive(false);
        centerCircle.gameObject.SetActive(false);
        cost.gameObject.SetActive(false);

    }

   
    // Update is called once per frame
    void Update()
    {
        if (studentManager.CheckWin())
        {
            ingredientText.gameObject.SetActive(false);
            dishText.gameObject.SetActive(false);
            budgetText.gameObject.SetActive(false);

            served.gameObject.SetActive(false);
            tastiness.gameObject.SetActive(false);
            cost.gameObject.SetActive(false);
            studentIndexText.gameObject.SetActive(false);
            moneyEarnedText.gameObject.SetActive(false);


            likedText.text = "Times food was liked: " + studentManager.likeCount;
            dislikeText.text = "Times food was disliked: " + studentManager.dislikeCount;
            okText.text = "Times food was ok: " + studentManager.okCount;
            moneyText.text = "Money Earned: $" + studentManager.moneyEarned;

            endScreen.gameObject.SetActive(true);
            finishText.gameObject.SetActive(true);
            likedText.gameObject.SetActive(true);
            dislikeText.gameObject.SetActive(true);
            okText.gameObject.SetActive(true);
            moneyText.gameObject.SetActive(true);




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
}
