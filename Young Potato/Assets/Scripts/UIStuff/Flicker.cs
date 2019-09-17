using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Flicker : MonoBehaviour
{

    public GameObject text;
    public int flickerAmount;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FlickerWarning()
    {
        for (int i = 0; i < flickerAmount; i++)
        {
            text.SetActive(true);
            yield return new WaitForSeconds(0.2f);
         
            text.SetActive(false);


            yield return new WaitForSeconds(0.2f);
            text.SetActive(true);
        }

        text.SetActive(false);
    }
}
