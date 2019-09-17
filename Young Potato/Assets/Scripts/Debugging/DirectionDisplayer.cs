using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionDisplayer : MonoBehaviour
{

    [Range(100,1000)]public float lineLength;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.right * lineLength);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.up * lineLength);

        Gizmos.color = Color.blue;


        Gizmos.DrawLine(transform.position, transform.forward * lineLength);
        //Gizmos.color = Color.green;
    }
}
