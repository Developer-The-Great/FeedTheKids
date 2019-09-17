using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{

    private MaterialPropertyBlock _propBlock;

    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();


    }

    // Update is called once per frame
    void Update()
    {
        // Get the current value of the material properties in the renderer.
        _renderer.GetPropertyBlock(_propBlock);
        // Assign our new value.
        _propBlock.SetColor("_pColor", Color.red);
        // Apply the edited values to the renderer.
        _renderer.SetPropertyBlock(_propBlock);
    }
}
