using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPosition
{
    ToFood,
    ToChildren,
    ToKnife
}

[RequireComponent(typeof(Camera))]
public class CameraRotator : MonoBehaviour
{

    private Quaternion defaultRotation;
    private Quaternion OtherRotation;
    private Quaternion knifeLookAtRotation;

    public Transform KnifeLookatPosition;

    [SerializeField]private float angleX;


    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = transform.rotation;

        OtherRotation = defaultRotation * Quaternion.AngleAxis(angleX, Vector3.right);

        knifeLookAtRotation = Quaternion.LookRotation(KnifeLookatPosition.position - transform.position);
        
    }

    public void RotateTowardsPosition(CameraPosition position)
    {
        switch (position)
        {
            case CameraPosition.ToChildren:
                transform.rotation = Quaternion.Slerp(transform.rotation,OtherRotation,0.05f);
                break;
            case CameraPosition.ToFood:
                transform.rotation = Quaternion.Slerp(transform.rotation, defaultRotation, 0.05f);
                break;
            case CameraPosition.ToKnife:
                transform.rotation = Quaternion.Slerp(transform.rotation, knifeLookAtRotation, 0.05f);
                break;
            
        }

    }

   
}
