using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Grabbable : MonoBehaviour
{
    [SerializeField] public Vector3 liftingOffset;

    protected Rigidbody rb;

    protected int initialLayerMask;
    protected const int ignoreLayer = 2;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialLayerMask = gameObject.layer;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void SetRayCastInvisibility(bool isInvisible)
    {
        if (isInvisible)
        {
            gameObject.layer = ignoreLayer;
        }
        else
        {
            gameObject.layer = initialLayerMask;
        }

    }

}
