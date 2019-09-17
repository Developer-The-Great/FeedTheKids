using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{

    public GameObject Obj;
    public Vector3 position;
    public KeyCode key;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            Instantiate(Obj, position, Quaternion.identity);
        }
    }
}
