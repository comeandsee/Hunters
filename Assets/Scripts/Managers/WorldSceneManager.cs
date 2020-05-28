﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSceneManager : AnimalSceneManager
{
    private GameObject animal;
    private AsyncOperation loadScene;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void playerTapped(GameObject player)
    {

    }

    public override void animalTapped(GameObject animal)
    {
        //		SceneManager.LoadScene(PocketDroidsConstants.SCENE_CAPTURE);
      //  List<GameObject> objects = new List<GameObject>();
     //   print(objects);
     //   objects.Add(animal);
    //    print(objects);
        SceneManager.LoadScene(AnimalsConstants.SCENE_CAPTURE, LoadSceneMode.Additive);
       // SceneTransitionManager.Instance.GoToScene(AnimalsConstants.SCENE_CAPTURE, objects);
    }
}

