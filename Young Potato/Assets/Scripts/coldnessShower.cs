using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coldnessShower : MonoBehaviour
{
    Food food;
    [SerializeField]private float coldness;
    ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!food)
        { food = transform.root.gameObject.GetComponent<Food>(); }
        coldness = food.Coldness;

        var main = particles.main;
        main.startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f -coldness);
    }
}
