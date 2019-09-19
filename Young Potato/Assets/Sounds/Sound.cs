﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Background
    public AudioSource BackgroundMusic;
    public AudioSource LunchroomBackground;


    //Voice Overs 
    public AudioSource ExpensiveIngredient;
    public AudioSource FoodGood;
    public AudioSource FoodOkay;
    public AudioSource FoodBad;
    public AudioSource Waiting;

    //Sound Effects
    public AudioSource CashRegister;
    public AudioSource CookingDone;
    public AudioSource Cutting;
    public AudioSource DropItem;
    public AudioSource Knife;
    public AudioSource Sizzling;
    public AudioSource SchoolBell;
    public AudioSource WaterBoiling;
    public AudioSource FireAlarm;
    public AudioSource Footsteps;


    // Start is called before the first frame update
    void Start()
    {
        BackgroundMusic.Play();
        LunchroomBackground.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DropItem.Play();
        } 
    }
}
