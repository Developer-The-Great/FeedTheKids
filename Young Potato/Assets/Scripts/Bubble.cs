using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    float bubbleTime;
    float speed;
    public float totalDist;

    public void Init(float pBubbleTime)
    {
        bubbleTime = pBubbleTime;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startBubble());
    }

    // Update is called once per frame
    void Update()
    {
        totalDist += speed;

        
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + totalDist, transform.position.z);
    }

    IEnumerator startBubble()
    {
        yield return new WaitForSeconds(bubbleTime);
        Destroy(this.gameObject);
    }

}
