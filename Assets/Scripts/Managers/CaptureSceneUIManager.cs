﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CaptureSceneUIManager : MonoBehaviour
{
    [SerializeField] private CaptureSceneManager manager;
    [SerializeField] private GameObject successScreen;
    [SerializeField] private GameObject gameScreen;

    [SerializeField] private Text HPCountText;
    
    private Animal animal;
    private void Awake()
    { 
        animal = FindObjectOfType<Animal>();
        Assert.IsNotNull(animal);
        Assert.IsNotNull(manager);
        Assert.IsNotNull(successScreen);
        Assert.IsNotNull(gameScreen);
    }

    private void Update()
    {
        switch (manager.Status)
        {
            case CaptureSceneStatus.InProgress:
                HandleInProgress();
                break;
            case CaptureSceneStatus.Successful:
                HandleSuccess();
                break;
            default:
                break;
        }
    }

    private void HandleSuccess()
    {
        UpdateVisibleScreen();
    }

    private void HandleInProgress()
    {
        UpdateVisibleScreen();
        updateHPCountText();
    }

    private void UpdateVisibleScreen()
    {
        successScreen.SetActive(manager.Status == CaptureSceneStatus.Successful);
        gameScreen.SetActive(manager.Status == CaptureSceneStatus.InProgress);
    }

    public void updateHPCountText()
    {
        HPCountText.text = animal.Hp.ToString();
    }
}
