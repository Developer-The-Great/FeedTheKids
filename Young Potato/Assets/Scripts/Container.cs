using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Container : MonoBehaviour
{

    protected Player player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetPlayer();

        Debug.Assert(player);
    }

    protected Player GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
}
