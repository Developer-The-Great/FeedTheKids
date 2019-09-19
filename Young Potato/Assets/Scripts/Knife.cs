﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Knife : Grabbable
{
    Player player;
    public Queue<Vector3> knifePositions;
    public float acceptableDist = 0.01f;
    public BoxCollider knifePart;
    private Food food;
    private SoundManager soundManager; 

    private Vector3 initialLimitingOffset;

    public bool isCutting;

    // Start is called before the first frame update
    void Start()
    {
        initialLimitingOffset = liftingOffset;
        transform.parent = null;
        rb = GetComponent<Rigidbody>();
        knifePositions = new Queue<Vector3>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }


    

    // Update is called once per frame
    void Update()
    {

        GameObject hoveredObj = player.ObjectCurrentlyHovered;
        if (hoveredObj)
        {

            food = hoveredObj.GetComponent<Food>(); ;
        }


        

        if (knifePositions.Count != 0)
        {
            GoTo();
        }
        else
        {
            liftingOffset = initialLimitingOffset;
            followObjectOrientation(transform.rotation);
            SetKinematic(false);

        }


        if(food && food.IsCutable() && isPlayerGrabbingKnife())
        {
            rb.angularVelocity = Vector3.zero;
            followObjectOrientation(food.transform.rotation);

        }
        
        if(player.grabber.grabbedObject != this)
        {
            liftingOffset = initialLimitingOffset;
            SetKinematic(false);
            knifePositions.Clear();
        }

       
    }

    private bool isPlayerGrabbingKnife()
    {
        if (player.grabber.grabbedObject != null)
        {
           
            soundManager.Knife.Play();
            return player.grabber.grabbedObject.GetComponent<Knife>();
        }
        else
        {
            return false;
        }
            
    }
    
    private void followObjectOrientation(Quaternion targetRotation)
    {
        
        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        if (angle >= 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.05f);
        }
    }

    public void Slice(Vector3 endPosition)
    {


        if (!food)
        {
            Debug.Log("No food found");
            return;
        }
        else if(food && food.isOnCuttingBoard)
        {
            knifePositions.Enqueue(new Vector3(endPosition.x,food.transform.position.y,endPosition.z));
            knifePositions.Enqueue(transform.position + liftingOffset);
        }

    }

    private void GoTo()
    {
        
        float distance = Vector3.Distance(transform.position + liftingOffset, knifePositions.Peek());

        if(distance < acceptableDist)
        {
            liftingOffset = knifePositions.Dequeue() - transform.position; ;
        }
        else
        {
            liftingOffset = Vector3.Lerp(liftingOffset, knifePositions.Peek() - transform.position, 0.1f);

        }
    }

    public void OnDrawGizmos()
    {
        if(knifePositions != null)
        {
            if (knifePositions.Count != 0)
            {
                foreach(Vector3 position in knifePositions)
                {
                    Gizmos.color = Color.black;
                    
                    Gizmos.DrawSphere(position, 0.1f);
                }


            }
        }
    }

    public void SetKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
    }

   
}
