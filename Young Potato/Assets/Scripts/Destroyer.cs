using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public bool isDestroyMode = false;
    // Start is called before the first frame update
    void Start()
    {
        isDestroyMode = true;
    }


    

    private void OnTriggerStay(Collider other)
    {

        if(isDestroyMode)
        {
            Destroy(other.gameObject);

        }
 
    }
    

    public IEnumerator StartDestroy()
    {
        isDestroyMode = true;
        yield return new WaitForSeconds(0.5f);
        isDestroyMode = false;

        Destroy(this);
    }

}
