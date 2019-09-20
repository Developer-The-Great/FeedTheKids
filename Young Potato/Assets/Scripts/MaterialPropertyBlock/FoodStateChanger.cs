using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStateChanger : MonoBehaviour
{
    private MaterialPropertyBlock _propBlock;

    private Renderer _renderer;

    private float burntness;
    private float friedness;
    private float boildness;

    Food food;

    void Start()
    {
        
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        if(!food)
        {
            food = transform.root.gameObject.GetComponent<Food>();
        }
       

        burntness = food.Burntness;
        boildness = food.Boildness;
        friedness = food.Friedness;

        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetFloat("_Friedness",friedness);
        _propBlock.SetFloat("_Boiledness", boildness);
        _propBlock.SetFloat("_Burntness", burntness);

        _renderer.SetPropertyBlock(_propBlock);


    }
}
