using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Chicken,
    Rice,
    Potato,
    Apple,
    Hamburger,
    None

}


[RequireComponent(typeof(CharacterController))]
public class Student : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 worldPositionInfoLocation;
    private GameObject selectedIndicator;
    private StudentManager studentManager;

    
    [SerializeField]private float statisfaction;


    [SerializeField]private Vector3 target;

    public bool setGoal;

    private float acceptableDistance;

    public float speed;
    //the student position in the line
    public int StudentIndex { get; private set; }
    //amount of money student has
    public float StudentBudget { get; private set; }

    public bool IsTiredOfWaiting { get; private set; }

    public FoodType preferedFood;

    [SerializeField] private float MaxWaitTime;
     private float timeWaited;

    public bool isBroke;

    [SerializeField] private float fixedYPosition;

    public void Init(float pBudget,float pStatisfaction,int pIndex,StudentManager pStudentManager,FoodType pRequest,float pMaxWaitTime)
    {
        StudentBudget = pBudget;
        statisfaction = pStatisfaction;
        StudentIndex = pIndex;
        studentManager = pStudentManager;
        MaxWaitTime = pMaxWaitTime;

        preferedFood = pRequest;

        isBroke = Mathf.Approximately(pBudget, 0);
    }

    public float GetWaitPercentage()
    {
        return Mathf.Clamp(timeWaited / MaxWaitTime,0,1.0f);
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        acceptableDistance = speed / 200;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaitTime();

        if (setGoal)
        {
            Vector3 direction = target - gameObject.transform.position;
            Move(GetYIgnoreVec(direction));
        }
    }

   

    public void Move(Vector3 direction)
    {
        characterController.SimpleMove(direction.normalized * speed);

        if(Vector3.Distance(GetYIgnoreVec(gameObject.transform.position), GetYIgnoreVec(target)) < acceptableDistance)
        {
            setGoal = false;
            gameObject.transform.position = GetYIgnoreVec(target);

            if(Vector3.Distance(target,studentManager.lineTransform[StudentIndex].position) < 0.01f)
            {

            }
        }
        
        if(Vector3.Distance(transform.position,studentManager.GetExitPosition()) < 1)
        {
            studentManager.DestroyStudent(this);
        }



    }

    Vector3 GetYIgnoreVec(Vector3 vec)
    {
        return new Vector3(vec.x, fixedYPosition, vec.z);
    }

    private void GoTo(Vector3 Position)
    {
        setGoal = true;
        target = Position;
    }

    public void EnterPlayArea(Vector3 position)
    {
        GoTo(position);


    }

    public void ExitPlayArea()
    {

        GoTo(studentManager.GetExitPosition());
    }

    public void UpdateWaitTime()
    {
        timeWaited += Time.deltaTime;

        if(timeWaited > MaxWaitTime)
        {
            IsTiredOfWaiting = true;
            timeWaited = MaxWaitTime;
        }
    }

  
}
