﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CaptureSceneUIManager : MonoBehaviour
{
    [SerializeField] private CaptureSceneManager manager;
    [SerializeField] private GameObject successScreen;
    [SerializeField] private GameObject failScreen;
    [SerializeField] private GameObject gameScreen;

    [SerializeField] private Text HPCountText;

    [SerializeField] private Text animalNameTxt;
    [SerializeField] private Text animalPointsTxt;


    private Animal animal;


    private void Awake()
    {
   
        animal = AnimalFactory.Instance.SelectedAnimal;

        Assert.IsNotNull(animal);
        Assert.IsNotNull(manager);
        Assert.IsNotNull(successScreen);
        Assert.IsNotNull(failScreen);
        Assert.IsNotNull(gameScreen);
        Assert.IsNotNull(animalNameTxt);
        Assert.IsNotNull(animalPointsTxt);
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
            case CaptureSceneStatus.Failed:
                HandleFail();
                break;
            default:
                break;
        }
    }

    private void HandleFail()
    {
        UpdateVisibleScreen();
    }

    private void HandleSuccess()
    {
        if (AnimalFactory.Instance.NameSelectedAnimal != null)
        {
            var animalName = AnimalFactory.Instance.NameSelectedAnimal.Replace("(Clone)", "");
            animalNameTxt.text = animalName;
            animalPointsTxt.text = "+ " + AnimalFactory.Instance.PointsSelectedAnimal.ToString();
        }
        UpdateVisibleScreen();

    }

    private void HandleInProgress()
    {
        UpdateVisibleScreen();
        updateHPCountText();
    }

    private void UpdateVisibleScreen()
    {
        failScreen.SetActive(manager.Status == CaptureSceneStatus.Failed);
        successScreen.SetActive(manager.Status == CaptureSceneStatus.Successful);
        gameScreen.SetActive(manager.Status == CaptureSceneStatus.InProgress);
    }

    public void updateHPCountText()
    {
        HPCountText.text = animal.Hp.ToString();
    }

   
}
