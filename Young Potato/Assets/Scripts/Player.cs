using System.Collections;
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

    public Tray[] tray; 

    private float DishCost;
    private float IngredientCost;
    private float Budget;

    public Student[] currentlyServing = new Student[3];

    public GameObject ObjectCurrentlyHovered { private set; get; }

    public FoodType currentType;

    public int currentStudentIndex { private set; get; }
    
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

    private Queue<int> positionToFillQueue;
    private Queue<int> fillStartIndexQueue;

    public int[] fillStartIndex;
    public int[] positionToFill;
    public int firstPosition;

    private void Awake()
    {

        UIManager = GetComponent<UIManager>();

        grabber = GetComponent<Grabber>();

        inputController = GameManager.Manager.Input;

        studentManager = GameObject.FindGameObjectWithTag("studentManager").GetComponent<StudentManager>();

        handObj = GameObject.FindGameObjectWithTag("hand");

        hand = handObj.GetComponent<Hand>();

        CamRotator = GetComponentInChildren<CameraRotator>();

        studentManager.OnStudentDestroy += checkIfCanFillPosition;
    }

    void checkIfCanFillPosition(int studentServed)
    {
        if(fillStartIndexQueue.Count != 0)
        {
            if(studentServed == fillStartIndexQueue.Peek())
            {
                fillStartIndexQueue.Dequeue();

                if(positionToFillQueue.Count != 0)
                {
                    fillPosition(positionToFillQueue.Dequeue(), false);
                }

            }
        }
    }

    void Start()
    {
        
        DishCost = 0;
        IngredientBudget = 0;

        Debug.Log("intialize player");

        //fillPosition(0,false);
        fillPosition(firstPosition, false);
        //fillPosition(2, false);

        StudentBudget = studentManager.studentBudget[0];
        UIManager.UpdateBudgetText();

        UIManager.UpdateRequestText();

        studentManager = GameObject.FindGameObjectWithTag("studentManager").GetComponent<StudentManager>();

        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        positionToFillQueue = new Queue<int>();
        fillStartIndexQueue = new Queue<int>();


        for (int i = 0; i < positionToFill.Length; i++)
        {
            positionToFillQueue.Enqueue(positionToFill[i]);
        }

        for (int i = 0; i < fillStartIndex.Length; i++)
        {
            fillStartIndexQueue.Enqueue(fillStartIndex[i]);
        }

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
                UIManager.racks[i].gameObject.SetActive(false);
                UIManager.bobble.transform.GetChild(i).gameObject.SetActive(false);
                UIManager.buttons.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                UIManager.racks[i].gameObject.SetActive(true);
                UIManager.bobble.transform.GetChild(i).gameObject.SetActive(true);
                UIManager.buttons.transform.GetChild(i).gameObject.SetActive(true);
                tray[i].transform.root.gameObject.SetActive(true);
                currentlyServing[i].trayInStudent.SetActive(false);
                
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

            ObjectCurrentlyHovered = HitObj;

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
                    currentStudentIndex = student.StudentIndex;
                    StudentBudget = student.StudentBudget;
                    DishCost = tray[student.StudentIndex].DishCost;
                    currentType = student.preferedFood;
                    UIManager.UpdateRequestText();
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
            else if (grabber.grabbedObject is Knife knife)
            {
                hand.SetGrabbingKnife();
                CamRotator.RotateTowardsPosition(CameraPosition.ToKnife);
            }


        }

        if(grabber.grabbedObject != null)
        {
            
            if (grabber.grabbedObject is Knife knife && Input.GetMouseButtonDown(1))
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
            fillPosition(currentStudentIndex,false);

        }
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
