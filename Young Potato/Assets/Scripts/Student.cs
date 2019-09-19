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
    Gogurt,
    Milk,
    None

}


[RequireComponent(typeof(CharacterController))]
public class Student : MonoBehaviour
{
    public GameObject trayInStudent;
    private CharacterController characterController;
    private Vector3 worldPositionInfoLocation;
    private GameObject selectedIndicator;
    private StudentManager studentManager;
    private SoundManager soundManager;
    
    [SerializeField]private Vector3 target;

    public bool setGoal;

    private bool hasComplained;

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
    public bool isServed;

    [SerializeField] private float fixedYPosition;

    private Animator animation;

    public void Init(float pBudget,int pIndex,StudentManager pStudentManager,FoodType pRequest,float pMaxWaitTime,GameObject model)
    {

        GameObject HumanModel = Instantiate(model,transform.position,Quaternion.identity);

        animation = HumanModel.GetComponentInChildren<Animator>();

        HumanModel.transform.parent = gameObject.transform;

        HumanModel.transform.localPosition = Vector3.zero;
        HumanModel.transform.localScale = new Vector3(1, 1, 1);

        StudentBudget = pBudget;
        
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
        acceptableDistance = speed / 50;

        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaitTime();

        if (setGoal)
        {
            MoveToGoal();
        }
        else
        {
            animation.SetBool("is_walking", false);
        }

        if(isServed)
        {
            trayInStudent.SetActive(true);
            soundManager.CashRegister.Play();
        }
    }

    private void MoveToGoal()
    {
        Vector3 direction = target - gameObject.transform.position;
        Move(GetYIgnoreVec(direction));
        FollowOrientation(target);
        animation.SetBool("is_walking", true);
    }

    public void FollowOrientation(Vector3 lookAt)
    {
        Quaternion targetRotation = Quaternion.LookRotation(GetYIgnoreVec(lookAt + -Vector3.forward * 3) - GetYIgnoreVec(transform.position), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.03f);

        if(Quaternion.Angle(targetRotation,transform.rotation) < 1f)
        {
            transform.rotation = targetRotation;

        }
    }


    public void Move(Vector3 direction)
    {
        characterController.SimpleMove(direction.normalized * speed);

        if (Vector3.Distance(GetYIgnoreVec(gameObject.transform.position), GetYIgnoreVec(target)) < acceptableDistance)
        {
            setGoal = false;
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

        if ( timeWaited >= 0.5f * MaxWaitTime && !hasComplained )
        {
            hasComplained = true;
            soundManager.Waiting.Play();

        }
        if(timeWaited > MaxWaitTime)
        {
            IsTiredOfWaiting = true;
            timeWaited = MaxWaitTime;

            
        }


    }

  
}
