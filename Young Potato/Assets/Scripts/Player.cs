﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Grabber),typeof(UIManager))]
public class Player : MonoBehaviour
{
    
    GameObject handObj;
    Hand hand;

    private CameraRotator CamRotator;
    [Range(0, 1)] public float YCameraChangeThreshold;

    public Grabber grabber { get; private set; }
    private StudentManager studentManager;
    private InputController inputController;
    private UIManager UIManager;
    private SoundManager soundManager;

    public Tray[] tray; //{ private set; get; }

    [SerializeField] private float DishCost;
    [SerializeField] private float IngredientCost;
    [SerializeField] private float Budget;

    [SerializeField] private int currentlySelectedStudent;

    public Student[] currentlyServing = new Student[3];

    public GameObject Selected;

    public FoodType currentType;

    public int  currentStudentIndex
    {
        get
        {
            return currentlySelectedStudent;
        }
        set
        {
            currentlySelectedStudent = value;
            
        }
    }

    public float StudentBudget
    {
        get
        {
            return Budget;
        }
        set
        {
            Budget = value;
            UIManager.UpdateBudgetText();
        }
    }

    public float DishBudget
    {
        get
        {
            return DishCost;
        }
        set
        {
            DishCost = value;
            UIManager.UpdateDishText();
        }

    }

    public float IngredientBudget
    {
        get
        {
            return IngredientCost;
        }
        set
        {

            IngredientCost = value;
            UIManager.UpdateIngredientText();

        }

    }

    private float distanceMultiplier = 1.0f;

    public bool CutMode;

    private void Awake()
    {
        UIManager = GetComponent<UIManager>();

        grabber = GetComponent<Grabber>();

        inputController = GameManager.Manager.Input;

        studentManager = GameObject.FindGameObjectWithTag("studentManager").GetComponent<StudentManager>();

        handObj = GameObject.FindGameObjectWithTag("hand");

        hand = handObj.GetComponent<Hand>();

        CamRotator = GetComponentInChildren<CameraRotator>();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
        DishCost = 0;
        IngredientBudget = 0;

        Debug.Log("intialize player");

        fillPosition(0,false);
        fillPosition(1, false);
        fillPosition(2, false);

        StudentBudget = studentManager.studentBudget[0];
        UIManager.UpdateBudgetText();

        UIManager.UpdateRequestText(currentType);

        studentManager = GameObject.FindGameObjectWithTag("studentManager").GetComponent<StudentManager>();



    }

    private void Update()
    {
        for(int i =0; i < currentlyServing.Length;i++)
        {
            if(currentlyServing[i] == null) { continue; }

            if(currentlyServing[i].IsTiredOfWaiting)
            {
                fillPosition(i,true);
            }

            if(currentlyServing[i].setGoal)
            {
                tray[i].transform.root.gameObject.SetActive(false);
            }
            else
            {
                tray[i].transform.root.gameObject.SetActive(true);
                currentlyServing[i].trayInStudent.SetActive(false);
                //
            }

        }


        if(Input.mousePosition.y > YCameraChangeThreshold*Screen.height)
        {
            CamRotator.RotateTowardsPosition(CameraPosition.ToChildren);
        }
        else
        {
            CamRotator.RotateTowardsPosition(CameraPosition.ToFood);
        }

        //UIManager.UpdateIndexText();
        UIManager.DeactivateFoodInformation();
        UIManager.UpdateDishText();

        Ray ScreenToWorld = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit Hit;

        bool mouseClickingOnObject = Input.GetMouseButton(0);
        bool mouseDownClickingObject = Input.GetMouseButtonDown(0);
        bool mouseHoverOnObject = Physics.Raycast(ScreenToWorld, out Hit);

        if (mouseHoverOnObject)
        {
            hand.DisableAll();

            grabber.MouseWorldPosition = Hit.point;
            hand.SetHandPosition(Hit.point);

            GameObject HitObj = Hit.transform.root.gameObject;
            Selected = HitObj;

            Grabbable grabbedObject = HitObj.GetComponent<Grabbable>();


            MicrowaveDoor door = Hit.transform.gameObject.GetComponent<MicrowaveDoor>();
            MicrowaveButton button = Hit.transform.gameObject.GetComponent<MicrowaveButton>();

            if(Hit.transform.gameObject.tag == "MicrowaveFloor")
            {
                distanceMultiplier = 0.5f;
            }
            else
            {
                distanceMultiplier = 1.0f;
            }


            if (grabbedObject)
            {
                handleGrabbable(grabbedObject, mouseClickingOnObject);
            }

            else if (door)
            {
                if (mouseDownClickingObject)
                {
                   
                    door.isOpen = !door.isOpen;
                    soundManager.MicrowaveDoor.Play();
                }
            }
            else if (button)
            {
                Selected = button.gameObject;
                if (mouseDownClickingObject)
                {
                    Debug.Log("microwave button");
                    button.addTime();
                }
            }
            else
            {

                Student student = HitObj.GetComponent<Student>();

                if (student)
                {
                    currentlySelectedStudent = student.StudentIndex;
                    StudentBudget = student.StudentBudget;
                    DishCost = tray[student.StudentIndex].DishCost;
                    currentType = student.preferedFood;
                    UIManager.UpdateRequestText(currentType);
                }
            }
        }
       
        if (Input.GetMouseButtonUp(0) && grabber.IsGrabbing)
        {
            grabber.stopGrabbing();

        }

        if (grabber.IsGrabbing)
        {
            grabber.holdGrabbable(distanceMultiplier);
            if (grabber.grabbedObject is Food)
            {
                hand.SetGrabbingFood();
            }
            else if (grabber.grabbedObject is Knife)
            {
                hand.SetGrabbingKnife();
            }


        }

        if(grabber.grabbedObject != null)
        {
            if (grabber.grabbedObject is Knife knife && Input.GetMouseButtonUp(1))
            {
                knife.isCutting = true;
                knife.Slice(grabber.MouseWorldPosition);
                knife.SetKinematic(true);
            }
        }
        

      
    }



    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            fillPosition(currentlySelectedStudent,false);

        }

        //DishBudget = tray[currentlySelectedStudent].DishCost;
    }

    private void handleGrabbable(Grabbable grabbable, bool mouseClickingOnObject)
    {
        hand.SetHovering(grabber.MouseWorldPosition);
        bool NotCurrentlyGrabbing = !grabber.IsGrabbing;

        if (grabbable is Food food)
        {
            UIManager.DisplayFoodInformation(food);

            if (NotCurrentlyGrabbing && mouseClickingOnObject)
            {
                
                decreaseBudget(food);
                grabber.grabGrabbable(food);
            }
           

        }
        else if (grabbable is Knife knife)
        {
            
            if (NotCurrentlyGrabbing && mouseClickingOnObject)
            {
                grabber.grabGrabbable(knife);
                

            }
            
            
        }
    }

    private void decreaseBudget(Food food)
    {
        if (food.InContainer)
        {
            IngredientBudget += food.FoodCost;
            food.InContainer = false;

        }

    }
    

    public void fillPosition(int positionIndex, bool isTimeChecking)
    {

        if(positionIndex == -1) { return; }

        Student previousStudent = currentlyServing[positionIndex];


        if (previousStudent)
        {

            if(!previousStudent.isBroke &&  tray[positionIndex].DishCost > previousStudent.StudentBudget && !isTimeChecking)
            {
                return;
            }

            if (Mathf.Approximately(tray[positionIndex].DishCost, 0) && previousStudent.isBroke)
                studentManager.starvingCount ++;
            
            Vector3 exitPoint = studentManager.GetExitPosition();

            previousStudent.ExitPlayArea();

            float statisfaction =  studentManager.GetStatisfaction(previousStudent, tray[positionIndex]);
            Debug.Log("statisfaction: " + statisfaction);
            Debug.Log("tray[positionIndex]: " + tray[positionIndex].DishCost);
            if (previousStudent.isBroke)
                studentManager.AddMoneyEarned(-statisfaction, tray[positionIndex].DishCost);
            else
                studentManager.AddMoneyEarned(statisfaction, tray[positionIndex].DishCost);
            

            StatisfactionState likeness = studentManager.FindLikeness(statisfaction);

            StartCoroutine(UIManager.displayServed(likeness));
            tray[positionIndex].GiveFood(previousStudent) ;

            previousStudent.isServed = true;

        }

        Student student;

        if(studentManager.CreateStudent(out student, positionIndex))
        {
            currentlyServing[positionIndex] = student;
        }
        else
        {
            currentlyServing[positionIndex] = null;
        }

        UIManager.UpdateBudgetText();
    }


    

  


   
}
