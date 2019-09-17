using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentStateChanger : MonoBehaviour
{

    private MaterialPropertyBlock _propBlock;

    private Renderer _renderer;

    private Student student;

    private float angeryness;

    // Start is called before the first frame update
    void Start()
    {
        student = transform.root.GetComponent<Student>();

        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        angeryness = student.GetWaitPercentage();


        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetFloat("_Angeryness", angeryness);

        _renderer.SetPropertyBlock(_propBlock);
    }
}
