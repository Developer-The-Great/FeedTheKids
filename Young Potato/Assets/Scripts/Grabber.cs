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
    }

    public void stopGrabbing()
    {
        Rigidbody grabRb = grabbedObject.GetComponent<Rigidbody>();
        grabRb.velocity = Vector3.zero;


        grabbedObject.SetRayCastInvisibility(false);
        grabbedObject = null;
        IsGrabbing = false;

        
    }

    public void holdGrabbable()
    {
        grabbedObject.transform.position = mouseWorldPosition + grabbedObject.liftingOffset;
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
