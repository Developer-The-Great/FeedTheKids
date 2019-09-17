using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{

    public GameObject bubble;

    public Transform startPoint;
    public float radius;

    public int bubblesPerPhase;

    [Range(0,1)]public float phaseTime;
    [Range(0, 0.5f)] public float maxSize;
    [Range(0, 0.5f)] public float minSize;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateBubbles());
    }

   

    IEnumerator GenerateBubbles()
    {
        while(true)
        {
            yield return new WaitForSeconds(phaseTime);
            
            for (int i = 0; i < bubblesPerPhase; i++)
            {
                float angle = Random.Range(0, 360);
                float scale = Random.Range(minSize, maxSize);
                float bubbleTime = Random.Range(0.1f, 0.75f);
                float forwardLength = Random.Range(0.05f, radius);

                startPoint.rotation *= Quaternion.AngleAxis(angle, Vector3.up);

                Vector3 bubblePos = startPoint.position + startPoint.forward * forwardLength;

                GameObject bubbleObj  = Instantiate(bubble, bubblePos, Quaternion.identity);
                bubbleObj.transform.localScale = new Vector3(scale, scale, scale);

                bubbleObj.GetComponent<Bubble>().Init(bubbleTime);

            }

        }
       
    }
}
