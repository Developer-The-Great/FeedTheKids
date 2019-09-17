using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    FoodScorer scorer;
    UIManager UIManager;

    public Transform[] lineTransform = new Transform[3];
    private Transform SpawnPoint;
    private Transform ExitPoint;

    public GameObject studentObj;
    public float maxWaitTime;

    private Queue<float> studentBudgetQueue;
    private Queue<FoodType> studentRequestQueue;
    private Queue<float> studentWaitTimeQueue; 

    private float money;

    public float moneyEarned
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            UIManager.UpdateMoneyEarnedText();
        }
    }

    public float[] studentBudgets;

    public FoodType[] studentRequest;

    public float[] nutritionGiven;

    private int currentStudent = 0;

    public int StudentsServed { private set; get; }

    public int StudentIndex
    {
        get
        {
            return currentStudent;
        }
        private set
        {
            currentStudent = value;
        }
    }

    

    public float[] studentBudget
    {
        get
        {
            return studentBudgets;
        }
        private set
        {
            studentBudgets = value;
        }
    }

    public float debugMoneyEarned;
    public float debugStudentsServed;

    public int okCount;
    public int likeCount;
    public int dislikeCount;


    public void Next()
    {
        currentStudent++;

    }

    public void AssignNutrition(float nutrition)
    {
        nutritionGiven[StudentIndex] = nutrition;
    }

    public void CreateStudent(out Student student, int positionFill)
    {
        if(studentBudgetQueue.Count != 0)
        {
            student = Instantiate(studentObj, SpawnPoint.position, Quaternion.identity).GetComponent<Student>();

            student.Init(getNextStudentBudget(), 0, positionFill, this,getNextStudentRequest(), getNextStudentWaitTime());
            student.EnterPlayArea(lineTransform[positionFill].position);
        }
        else
        {
            student = null;


        }
        
    }

    private void Awake()
    {
        scorer = GameObject.FindGameObjectWithTag("scorer").GetComponent<FoodScorer>();
        SpawnPoint = GameObject.FindGameObjectWithTag("spawn").GetComponent<Transform>();
        ExitPoint = GameObject.FindGameObjectWithTag("exit").GetComponent<Transform>();
        UIManager = GameObject.FindGameObjectWithTag("Player").GetComponent<UIManager>();

        studentBudgetQueue = new Queue<float>();
        studentRequestQueue = new Queue<FoodType>();
        studentWaitTimeQueue = new Queue<float>();
        

        nutritionGiven = new float[studentBudgets.Length];

        for (int i = 0; i < studentBudgets.Length; i++)
        {
           studentBudgetQueue.Enqueue(studentBudget[i]);
           
        }


        for (int i = 0; i < studentBudgets.Length; i++)
        {
            studentRequestQueue.Enqueue(studentRequest[i]);
        }

        for (int i = 0; i < studentBudgets.Length; i++)
        {
            studentWaitTimeQueue.Enqueue(40 - (studentBudget[i]));
        }

        StudentsServed = 0;

    }

   private float getNextStudentWaitTime()
    {
        return studentWaitTimeQueue.Dequeue();
    }


    private float getNextStudentBudget()
    {
        return studentBudgetQueue.Dequeue();
    }

    private FoodType getNextStudentRequest()
    {
        return studentRequestQueue.Dequeue();
    }

    public Vector3 GetSpawnPosition()
    {
        return SpawnPoint.position;
    }
    public Vector3 GetExitPosition()
    {
        return ExitPoint.position;

    }

    private void Update()
    {
        debugMoneyEarned = moneyEarned;
        debugStudentsServed = StudentsServed;
    }

    public void DestroyStudent(Student student)
    {
        StudentsServed++;
        Destroy(student.gameObject);
    }

    public float GetStatisfaction(Student student,Tray tray)
    {
        return scorer.GetFoodMultiplier(student, tray.foodInTray);
    }

    public bool CheckWin()
    {
        return StudentsServed == studentBudgets.Length;
    }
    public StatisfactionState FindLikeness(float statisfaction)
    {
        Debug.Log("stat " + statisfaction);
        if (statisfaction < 1.0f)
        {
            dislikeCount++;
            return StatisfactionState.Dislike;
        }
        else if (statisfaction > 1.9f)
        {
            likeCount++;
            return StatisfactionState.Like;
        }
        else
        {
            okCount++;
            return StatisfactionState.Ok;
        }
    }
    
    public void AddMoneyEarned(float statisfaction,float studentDishCost)
    {
        moneyEarned += (statisfaction * studentDishCost);
    }
}


