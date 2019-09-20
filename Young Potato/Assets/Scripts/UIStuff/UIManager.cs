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

    public SoundManager soundManager;

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
    private Image[] requests = new Image[3];


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
    public Transform[] racks;
    [SerializeField] private GameObject[] bBills1;
    [SerializeField] private GameObject[] bBills2;
    [SerializeField] private GameObject[] bBills3;
    [SerializeField] private GameObject[] tBills1;
    [SerializeField] private GameObject[] tBills2;
    [SerializeField] private GameObject[] tBills3;

    private GameObject earnings;
    public GameObject buttons;

    private bool checkedWin = false;
    private GameObject[] stars = new GameObject[5];
    public Sprite overstar;

    private GameObject[] spots = new GameObject[5];

    private Transform[] billHolders = new Transform[3];
    private Transform[] buttonHolders = new Transform[3];
    private Transform[] bobbleHolders = new Transform[3];

    public Sprite[] images = new Sprite[5];
    public GameObject bobble;

    private GameObject detailToggle;

    [SerializeField] private Texture2D cursor;
    
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

//        ingredientText = GameObject.FindGameObjectWithTag("ingredientText").GetComponent<Text>();

        served = GameObject.FindGameObjectWithTag("servingText").GetComponent<Text>();

        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

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
        
        UpdateMoneyEarnedText();

        finishText = GameObject.Find("Finish").GetComponent<Text>();
        likedText = GameObject.Find("likedText").GetComponent<Text>();
        dislikeText = GameObject.Find("dislikeText").GetComponent<Text>();
        okText = GameObject.Find("okText").GetComponent<Text>();
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        
        bobble = GameObject.Find("Bobbles");
        for (int i = 0; i < 3; i++)
        {
            requests[i] = bobble.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>();
        }

        earnings = GameObject.FindGameObjectWithTag("Earnings");
        buttons = GameObject.FindGameObjectWithTag("Buttons");

        stars = GameObject.FindGameObjectsWithTag("Star");
        spots = GameObject.FindGameObjectsWithTag("TranStar");

        Transform billHolder = GameObject.FindGameObjectWithTag("BillHolder").transform;
        Transform buttonHolder = GameObject.FindGameObjectWithTag("ButtonHolder").transform;
        Transform bobbleHolder = GameObject.FindGameObjectWithTag("BobbleHolder").transform;
        for (int i = 0; i < 3; i++)
        {
            billHolders[i] = billHolder.GetChild(i);
            buttonHolders[i] = buttonHolder.GetChild(i);
            bobbleHolders[i] = bobbleHolder.GetChild(i);
        }

        detailToggle = GameObject.FindGameObjectWithTag("DetailToggle");
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
        detailToggle.SetActive(false);
        
        Cursor.SetCursor(cursor, new Vector2(4.0f,0), CursorMode.Auto);
       
    }

    public void UpdateIngredientText()
    {
//        ingredientText.text = "Ingredient Cost: " + player.IngredientBudget;
    }

    public void UpdateDishText()
    {
        GameObject[][] bRacks = {bBills1, bBills2, bBills3};
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                bRacks[i][j].GetComponent<Image>().fillAmount = 0;
                bRacks[i][j].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            if (player.currentlyServing[i] == null) continue;
            if (player.tray[i].DishCost > 10 || player.tray[i].DishCost > player.currentlyServing[i].StudentBudget)
            {
                for (int j = 0; j < player.currentlyServing[i].StudentBudget; j++)
                {
                    bRacks[i][j].GetComponent<Image>().fillAmount = 1;
                    bRacks[i][j].GetComponent<Image>().color = new Color(1.0f, 0, 0, 1.0f);
                }


                soundManager.ExpensiveIngredient.Play();

            }
            else
            {
                if (player.tray[i].DishCost % 1 == 0)
                {
                    for (int j = 0; j < player.tray[i].DishCost; j++)
                    {
                        bRacks[i][j].GetComponent<Image>().fillAmount = 1;
                    }
                }
                else
                {
                    for (int j = 0; j < player.tray[i].DishCost - 1; j++)
                    {
                        bRacks[i][j].GetComponent<Image>().fillAmount = 1;
                        if (j == player.tray[i].DishCost - 1)
                        {
                            bRacks[i][j + 1].GetComponent<Image>().fillAmount = player.tray[i].DishCost % 1;
                        }
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
                soundManager.FoodBad.Play();
                soundManager.Footsteps.Play();
                soundManager.CashRegister.Play();
                break;
            case StatisfactionState.Like:
                tastiness.text = "The student liked that!";
                soundManager.FoodGood.Play();
                soundManager.Footsteps.Play();
                soundManager.CashRegister.Play();
                break;
            case StatisfactionState.Ok:
                tastiness.text = "The student was ok with it...";
                soundManager.FoodOkay.Play();
                soundManager.Footsteps.Play();
                soundManager.CashRegister.Play();
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

    public void UpdateRequestText()
    {
        for (int i = 0; i < 3; i++)
        {
            FoodType foodType = player.currentlyServing[i].preferedFood;
            switch (foodType)
            {
                case FoodType.Apple:
                    requests[i].sprite = images[0];
                    break;
                case FoodType.Chicken:
                    requests[i].sprite  = images[1];
                    break;
                case FoodType.Potato:
                    requests[i].sprite  = images[2];
                    break;
                case FoodType.Rice:
                    requests[i].sprite  = images[3];
                    break;
                case FoodType.None:
                    requests[i].sprite  = images[4];
                    break;

            }
        }



    }


    public void DisplayFoodInformation(Food food)
    {
        if (!checkedWin)
        {
            //
            //
            Vector2 mousePosition = new Vector2(Input.mousePosition.x * scaler.referenceResolution.x / Screen.width,
                Input.mousePosition.y * scaler.referenceResolution.y / Screen.height);

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
        Camera mainCamera = Camera.main;
        for (int i = 0; i < 3; i++)
        {
            racks[i].position = mainCamera.WorldToScreenPoint(billHolders[i].position);
            bobble.transform.GetChild(i).position = mainCamera.WorldToScreenPoint(bobbleHolders[i].position);
            buttons.transform.GetChild(i).position = mainCamera.WorldToScreenPoint(buttonHolders[i].position);
        }
        
        if (studentManager.CheckWin() && !checkedWin)
        {
            checkedWin = true;
            bManager.gameObject.SetActive(false);
            earnings.gameObject.SetActive(false);
            buttons.gameObject.SetActive(false);

            served.gameObject.SetActive(false);
            tastiness.gameObject.SetActive(false);
            cost.gameObject.SetActive(false);
            moneyEarnedText.gameObject.SetActive(false);
            DeactivateFoodInformation();
            bobble.SetActive(false);

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
            detailToggle.SetActive(true);
            moneyText.gameObject.SetActive(true);
            starvingText.gameObject.SetActive(true);

            int star = 0;
            if (earned >= 10.0f)
                star++;
            if (earned >= 20.0f)
                star++;
            if (earned >= 30.0f)
                star++;
            if (studentManager.dislikeCount <= 2)
                star++;
            if (studentManager.likeCount >= 4)
                star++;
            if (studentManager.starvingCount == 0)
                star++;
            for (int i = 0; i < star; i++)
            {
                if(i < 6)
                    LeanTween.move(stars[i], spots[i].transform, 3.0f);
                else
                {
                    stars[0].GetComponent<Image>().sprite = overstar;
                    stars[1].GetComponent<Image>().sprite = overstar;
                    stars[2].GetComponent<Image>().sprite = overstar;
                    stars[3].GetComponent<Image>().sprite = overstar;
                    stars[4].GetComponent<Image>().sprite = overstar;
                }
            }
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

    public void toggleDetails()
    {
        bool stateToSet = !okText.gameObject.activeInHierarchy;
        okText.gameObject.SetActive(stateToSet);
        likedText.gameObject.SetActive(stateToSet);
        dislikeText.gameObject.SetActive(stateToSet);
    }
}
