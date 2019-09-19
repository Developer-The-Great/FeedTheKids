using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Grabber : MonoBehaviour
{


    private Player player;
    private InputController inputController;
 

    public Vector3 Offset;

    private Vector3 mouseWorldPosition;

    public Grabbable grabbedObject { get; private set; }
    private SoundManager soundManager;

    public bool IsGrabbing;

    public Vector3 MouseWorldPosition
    {
        get
        {
            return mouseWorldPosition;
        }
        set
        {
            mouseWorldPosition = value;

        }
    }
    

    private void Awake()
    {
        inputController = GameManager.Manager.Input;
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();


    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();

        Debug.Assert(player);


    }

    public void grabGrabbable(Grabbable grab)
    {
        grab.SetRayCastInvisibility(true);
        

        grabbedObject = grab;
        IsGrabbing = true;

        if ( IsGrabbing == true && grabbedObject is Knife knife )
        {
            soundManager.Knife.Play();
        }
    }

    public void stopGrabbing()
    {
        Rigidbody grabRb = grabbedObject.GetComponent<Rigidbody>();
        grabRb.velocity = Vector3.zero;


        grabbedObject.SetRayCastInvisibility(false);
        grabbedObject = null;
        IsGrabbing = false;
        
        if (IsGrabbing == false)
        {
            soundManager.DropItem.Play();
        }
        

        
    }

    public void holdGrabbable(float distanceMultiplier)
    {
        grabbedObject.transform.position = mouseWorldPosition + grabbedObject.liftingOffset * distanceMultiplier;
        grabbedObject.transform.rotation = Quaternion.identity;
        grabbedObject.GetComponent<Grabbable>().stopAngularVelocity();
    }

    


    private void OnDrawGizmos()
    {
        if (mouseWorldPosition != null)
        {
            Gizmos.DrawSphere(mouseWorldPosition, 0.1f);
        }

    }
}
