using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Food : Grabbable
{
    [SerializeField] [Range(0,1)] float addedSeperation;
    [SerializeField] private FoodType foodType;



    [SerializeField] private float BurnCoefficient = 1f;
    float add;

    [Range(0.0f, 100.0f)] [SerializeField] private float cookTime;

    public float Friedness { private set; get; }

    public float Boildness { private set; get; }

    public float Burntness { private set; get; }

    public float Coldness { private set; get; }


    public float showFried;
    public float showBoildness;
    public float showBurntness;
    public float showColdness;

    public FoodType Type
    {
        get
        {
            return foodType;
        }
        set
        {
            foodType = value;
        }
    }

    public Transform[] cutLocations;
    public GameObject[] foodBits;

    public Vector3 offset;
    public Vector3 cutOffset;

    public bool InContainer;
    public bool isOnCuttingBoard;


    public Vector3 initialLoc;

    [SerializeField]private float costPerPiece;

    public float FoodCost
    {
        get
        {
            return costPerPiece * foodBits.Length;
        }
        private set
        {
            costPerPiece = value;
        }
    }

    ///test
    public void Init(Vector3 pPositon,GameObject[] pFoodBits,Transform[] pCutLocations,
        Vector3 pOffset,Vector3 pCutOffset,Vector3[] localScale,
        Vector3 pLiftingOffset,float pCostPerPiece,Quaternion pRotation,float pCookTime,Vector3 pInitialLoc,float pSeperation)
    {
        cookTime = pCookTime;

        addedSeperation = pSeperation;

        initialLoc = pInitialLoc;

        costPerPiece = pCostPerPiece;
        cutOffset = pCutOffset;

        Debug.Log("init called");

        liftingOffset = pLiftingOffset;

        

        foodBits = pFoodBits;
        cutLocations = pCutLocations;

        offset = pOffset;

        Vector3 foodLocation = initialLoc;

        for (int i =0; i < foodBits.Length;i++)
        {
            foodBits[i].transform.position = Vector3.zero;
            foodBits[i].transform.rotation = Quaternion.identity;
            
            foodBits[i].transform.parent = gameObject.transform;
            foodBits[i].transform.localScale = localScale[i];
            foodBits[i].transform.localPosition = foodLocation;
            
            foodLocation += offset;


        }

        Vector3 cutLocation = Vector3.zero;

        for (int i = 0; i < pCutLocations.Length; i++)
        {
            pCutLocations[i].transform.position = Vector3.zero;
            pCutLocations[i].transform.rotation = Quaternion.identity;

            pCutLocations[i].transform.parent = gameObject.transform;
            pCutLocations[i].transform.localPosition = cutLocation;

            cutLocation += cutOffset;

        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        transform.position = pPositon;
        transform.rotation = pRotation;


        cutLocations[0].transform.localPosition -= cutOffset;
        cutLocations[cutLocations.Length - 1].transform.localPosition += cutOffset;
        //boo
    }

    private void Awake()
    {
        InContainer = true;
    }

    void Start()
    {
        add = 1 / cookTime;
        rb = GetComponent<Rigidbody>();
        transform.parent = null;
       
    }

    private void Update()
    {
        showFried = Friedness;
        showBoildness = Boildness;
        showColdness = Coldness;
        showBurntness = Burntness;
    }

    public bool IsCutable()
    {
        return foodBits.Length > 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "frying")
        {

            fry();
        }
        else if(other.tag == "boiling")
        {
            boil();
        }
        else if(other.tag == "cuttingBoard" && foodBits.Length > 1)
        {

            Vector3 right = Vector3.Cross(transform.forward, Vector3.up);
            Vector3 forward = Vector3.Cross(Vector3.up, right);

            Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);

            float angle = Quaternion.Angle(transform.rotation, targetRotation) ;


            if (angle >= 1.5f)
            {

                float lerpVal = 0.05f;
                if(isOnCuttingBoard)
                {
                    lerpVal = 1.0f;
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpVal);
            }

          
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "knife")
        {
            Knife knife = collision.gameObject.GetComponent<Knife>();

            if(knife.isCutting)
            {
                Vector3 contactPoint = collision.GetContact(0).point;

                if (collision.contactCount >= 2)
                {
                    contactPoint = Vector3.Lerp(collision.GetContact(0).point, collision.GetContact(1).point, 0.5f);
                }
                
                Cut(contactPoint);
                knife.isCutting = false;
            }

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
        if(collision.gameObject.tag == "cuttingBoard")
        {
            isOnCuttingBoard = true;
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "cuttingBoard")
        {

            isOnCuttingBoard = false;
            rb.constraints = RigidbodyConstraints.None;

        }
    }



 

    public override void SetRayCastInvisibility(bool isInvisible)
    {
        if(isInvisible)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer =  ignoreLayer;
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = initialLayerMask;
            }
        }
        
    }

    public float GetReadiness()
    {
        float readiness = Friedness;

        if (Boildness > Friedness)
        {
            readiness = Boildness;
        }
        return readiness;
    }

    private void fry()
    {
        if (Friedness > 1.0f)
        {
            Burntness += add * Time.deltaTime;

        }
        else
        {
            Friedness += add * Time.deltaTime;
        }
    }
    private void boil()
    {
        if (Boildness > 1.0f)
        {
            Burntness += add * Time.deltaTime;

        }
        else
        {
            Boildness += add * Time.deltaTime;
        }
    }

    public void Cut(Vector3 knifeCutLocation)
    {
        Debug.Log("called Cut");

        float smallestDistance = float.MaxValue;

        Transform foundCutLocation =null;
        int FoodBitIndex = 0;
        
        //find cut point that is closest to the knifeCutLocation
        for(int i =0;i < cutLocations.Length;i++)
        {
            float knifeToCutLocation = Vector3.Distance(cutLocations[i].position, knifeCutLocation);

            if (knifeToCutLocation < smallestDistance)
            {
                foundCutLocation = cutLocations[i];
                smallestDistance = knifeToCutLocation;
                FoodBitIndex = i;
            }
         
        }


        if(foundCutLocation == cutLocations[0] && foundCutLocation == cutLocations[cutLocations.Length-1])
        {
            return;
        }


        GameObject[] firstFoodBit = new GameObject[FoodBitIndex];

        Vector3[] foodLocalScale = new Vector3[firstFoodBit.Length];

        for (int i = 0; i < firstFoodBit.Length; i++)
        {
            firstFoodBit[i] = Instantiate(foodBits[i]);
            foodLocalScale[i] = foodBits[i].transform.localScale;

        }

        //find its transforms
        Transform[] firstCutLocations = new Transform[FoodBitIndex + 1];

        for (int i = 0; i < firstCutLocations.Length; i++)
        {
            firstCutLocations[i] = Instantiate(cutLocations[i]);
        }



        GameObject firstFoodObj = new GameObject();


        firstFoodObj.transform.localScale = transform.localScale;
        //firstFoodObj.transform.rotation = transform.rotation;

        Food firstFoodPiece = firstFoodObj.AddComponent<Food>();
        firstFoodPiece.Init(transform.position, firstFoodBit, firstCutLocations, 
            offset, cutOffset, foodLocalScale,
            liftingOffset,costPerPiece, transform.rotation,
            cookTime,initialLoc,addedSeperation);


        ///////////////create other cut 
        ///
        GameObject[] otherFoodBit = new GameObject[foodBits.Length - FoodBitIndex];
        Vector3[] otherFoodLocalScale = new Vector3[otherFoodBit.Length];

        for (int i = FoodBitIndex; i < foodBits.Length; i++)
        {
            otherFoodBit[i - FoodBitIndex] = Instantiate(foodBits[i]);
            otherFoodLocalScale[i - FoodBitIndex] = foodBits[i].transform.localScale;

        }

        //find its transforms
        Transform[] otherCutLocations = new Transform[foodBits.Length - FoodBitIndex + 1];

        for (int i = 0; i < otherCutLocations.Length; i++)
        {
            otherCutLocations[i] = Instantiate(cutLocations[i]);
        }

        GameObject otherFoodObj = new GameObject();

        otherFoodObj.transform.localScale= transform.localScale;

        
        Food otherFoodPiece = otherFoodObj.AddComponent<Food>();

        Vector3 newPosition = cutLocations[FoodBitIndex].transform.position + transform.right * addedSeperation;

        otherFoodPiece.Init(newPosition, otherFoodBit, otherCutLocations, 
            offset, cutOffset, otherFoodLocalScale, 
            liftingOffset,costPerPiece,transform.rotation,
            cookTime, initialLoc, addedSeperation);

        Destroy(gameObject);

    }

    

}
