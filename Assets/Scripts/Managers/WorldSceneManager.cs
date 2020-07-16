using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class WorldSceneManager : HuntersSceneManager
{
    private GameObject animal;
    private AsyncOperation loadScene;
    public bool isAreaGame = true;

    [SerializeField] Camera mapCam;
    [SerializeField] GameObject ARCam;
    [SerializeField] GameObject Map;
    private void Awake()
    {
        Assert.IsNotNull(mapCam);
        Assert.IsNotNull(ARCam);
        Assert.IsNotNull(Map);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CurrentPlayer.EndGame)
        {
            EndGame();
            GameManager.Instance.CurrentPlayer.EndGame = false;
        }

        if (GameManager.Instance.CurrentPlayer.NewLvl)
        {
            NewLvl();
            GameManager.Instance.CurrentPlayer.NewLvl = false;
        }
    }

    private void EndGame()
    {
        var uI = FindObjectOfType<UIManager>();
        uI.showWinnerBox();
    }

    public override void playerTapped(GameObject player)
    {

    }

    public override void animalTapped(GameObject animalObject)
    {
        //get tapped animal
        Animal animal = animalObject.GetComponent<Animal>();
        AnimalFactory.Instance.AnimalWasSelected(animal);

        var arrayOfChildrenOfAnimal = animal.transform
            .Cast<Transform>()
            .Where(c => c.gameObject.tag == "Animal").Select(c => c.gameObject)
            .ToArray();

        if (isAreaGame)
        {
            mapCam.enabled = false;
            ARCam.SetActive(true);
            Map.SetActive(false);
        }
        else // this is game for the closest distance around the user
        {
            // hide all others obj
            Animal[] allAnimals = FindObjectsOfType<Animal>();
            foreach (Animal a in allAnimals)
            {
                a.hideObject();
            }

            //show tapped animal
            foreach (GameObject childObj in arrayOfChildrenOfAnimal)
            {
                var child = childObj.GetComponent<Animal>();
                child.showObject();
            }

            animal.showObject();

            //go to capture scene
            SceneTransitionManager.Instance.
                GoToScene(HuntersConstants.SCENE_CAPTURE, new List<GameObject>());
        }
       
   }

    private void NewLvl()
    {
        var uI = FindObjectOfType<UIManager>();
        uI.showNewLvlBox();
        StartCoroutine(uI.WaitAndHideNewLvlBox(2.0f));
    }



}

