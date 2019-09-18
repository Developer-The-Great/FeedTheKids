using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Grabber),typeof(UIManager))]
public class Player : MonoBehaviour
{

    public Grabber grabber { get; private set; }
    private StudentManager studentManager;
    private InputController inputController;
    private UIManager UIManager;

    public Tray[] tray; //{ private set; get; }

    [SerializeField] private float DishCost;
    [SerializeField] private float IngredientCost;
    [SerializeField] private float Budget;

    [SerializeField] private int currentlySelectedStudent;

    public Student[] currentlyServing = new Student[3];

    public GameObject Selected { get; private set; }

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

    public bool CutMode;

    private void Awake()
    {
        UIManager = GetComponent<UIManager>();

        grabber = GetComponent<Grabber>();

        inputController = GameManager.Manager.Input;

        studentManager = GameObject.FindGameObjectWithTag("studentManager").GetComponent<StudentManager>();
       
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

    }

    private void Update()
    {
        for(int i =0; i < currentlyServing.Length;i++)
        {
            if(currentlyServing[i] == null) { return; }

            if(currentlyServing[i].IsTiredOfWaiting)
            {
                fillPosition(i,true);
            }
        }


        UIManager.UpdateIndexText();
        UIManager.DeactivateFoodInformation();
        UIManager.UpdateDishText();

        Ray ScreenToWorld = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit Hit;

        bool mouseClickingOnObject = Input.GetMouseButton(0);
        bool mouseHoverOnObject = Physics.Raycast(ScreenToWorld, out Hit);

        if (mouseHoverOnObject)
        {
            grabber.MouseWorldPosition = Hit.point;
            

            GameObject HitObj = Hit.transform.root.gameObject;
            Selected = HitObj;

            Grabbable grabbedObject = HitObj.GetComponent<Grabbable>();

            if(grabbedObject)
            {
                handleGrabbable(grabbedObject, mouseClickingOnObject);
            }
            else
            {
                
                Student student = HitObj.GetComponent<Student>();
                
                if(student)
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
            grabber.holdGrabbable();

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



    private void fillPosition(int positionIndex, bool isTimeChecking)
    {

        if(positionIndex == -1) { return; }

        Student previousStudent = currentlyServing[positionIndex];


        if (previousStudent)
        {

            if(!previousStudent.isBroke &&  tray[positionIndex].DishCost > previousStudent.StudentBudget && !isTimeChecking)
            {

                return;
            }

            Vector3 exitPoint = studentManager.GetExitPosition();

            previousStudent.ExitPlayArea();

            float statisfaction =  studentManager.GetStatisfaction(previousStudent, tray[positionIndex]);
            Debug.Log("statisfaction: " + statisfaction);
            Debug.Log("tray[positionIndex]: " + tray[positionIndex].DishCost);
            studentManager.AddMoneyEarned(statisfaction, tray[positionIndex].DishCost);

            StatisfactionState likeness = studentManager.FindLikeness(statisfaction);

            StartCoroutine(UIManager.displayServed(likeness));
            tray[positionIndex].DestroyFood();


            
        }

        Student student;

        studentManager.CreateStudent(out student, positionIndex);

        currentlyServing[positionIndex] = student;

        UIManager.UpdateBudgetText();
    }


    

  


   
}
