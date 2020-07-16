using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using static HuntersConstants;

public class WorldSceneManager : HuntersSceneManager
{
    private GameObject animal;
    private AsyncOperation loadScene;
   [SerializeField] private GameObject ARCam;



    private void Awake()
    {
       
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
        Animal animal;
        GameObject[] arrayOfChildrenOfAnimal;
        GetTappedAnimal(animalObject, out animal, out arrayOfChildrenOfAnimal);

        if (HuntersConstants.isAreaGame)
        {
            //todo

            //TODO
            if (ARCam.activeSelf)
            {
                AnimalFactory.Instance.AnimalWasSelected(animal);
                int startHp = animal.Hp;


                Harvest harvest = FindObjectOfType<Harvest>();
                harvest.GatherAnimal = true;


            }
            else
            {
                var uI = FindObjectOfType<UIManager>();
                uI.showHuntBox();
                StartCoroutine(WaitAndNotShowTxt(1.0f));
            }

        }
        else // this is game for the closest distance around the user
        {
            AnimalFactory.Instance.AnimalWasSelected(animal);

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

    private static void GetTappedAnimal(GameObject animalObject, out Animal animal, out GameObject[] arrayOfChildrenOfAnimal)
    {
        animal = animalObject.GetComponent<Animal>();

        arrayOfChildrenOfAnimal = animal.transform
            .Cast<Transform>()
            .Where(c => c.gameObject.tag == "Animal").Select(c => c.gameObject)
            .ToArray();
    }

    private void NewLvl()
    {
        var uI = FindObjectOfType<UIManager>();
        uI.showNewLvlBox();
        StartCoroutine(uI.WaitAndHideNewLvlBox(2.0f));
    }


    private IEnumerator WaitAndNotShowTxt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        var uI = FindObjectOfType<UIManager>(); ;
        uI.showHuntBox(false);

    }


   


  
}

