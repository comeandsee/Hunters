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

    private bool searchForNewAnimal = true;

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
        if (ARCam.activeSelf)
        {
            huntAnimal();
        }
    }


    private void huntAnimal()
    {
        if (ARCam.activeSelf && searchForNewAnimal)
        {
            var tuple = getAnimalWithMinDistance();
            Animal searchingAnimal = tuple.Item1;
            double startDistance = tuple.Item2;

            var playerPosition = getPlayerPosition();
            var updatedDistance = calculateDistance(playerPosition, searchingAnimal.transform.position);

            var distanceState = updateDistanceStatus(updatedDistance);

            var uI = FindObjectOfType<UIManager>();

            switch (distanceState)
            {
                case distanceZone.close:
                    uI.updateDistanceInf("look around");
                    break;
                case distanceZone.middle:
                    uI.updateDistanceInf("you are near");
                    break;
                case distanceZone.away:
                    uI.updateDistanceInf("come closer");
                    break;
                case distanceZone.tooFar:
                    uI.updateDistanceInf("you are far away");
                    break;
                default:
                    break;
            }

            searchForNewAnimal = false;
        }
    }

    private distanceZone updateDistanceStatus(double updatedDistance)
    {
        distanceZone distanceState = distanceZone.tooFar;

        if (updatedDistance <= (double)distanceZone.close)
        {
            distanceState = distanceZone.close;
        }
        else if (updatedDistance <= (double)distanceZone.middle)
        {
            distanceState = distanceZone.middle;
        }
        else if (updatedDistance <= (double)distanceZone.away)
        {
            distanceState = distanceZone.away;
        }

        return distanceState;
    }

    private void EndGame()
    {
        var uI = FindObjectOfType<UIManager>();
        uI.showWinnerBox();
    }

    public override void playerTapped(GameObject player)
    {
           var playerPosition = getPlayerPosition();
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


    private Tuple<Animal,double> getAnimalWithMinDistance()
    {
        var playerPosition = getPlayerPosition();
        var liveAnimals = AnimalFactory.Instance.LiveAnimals;

        Animal animalWithMaxDistance = null;
        double maxDistance;

        //first animal
        var animalPosition = liveAnimals[0].transform.position;
        maxDistance = calculateDistance(playerPosition, animalPosition);


        //find min distance
        foreach ( Animal liveAnimal in liveAnimals)
        {
            animalPosition = liveAnimal.transform.position;

            var distance = calculateDistance(playerPosition, animalPosition);

            if (distance <= maxDistance)
            {
                animalWithMaxDistance = liveAnimal;
                maxDistance = distance;
            }
        }


        return Tuple.Create(animalWithMaxDistance, maxDistance);
    }

    private Vector3 getPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        return player.transform.position;
    }

    private double calculateDistance( Vector3 object1, Vector3 object2 )
    {
        return Math.Sqrt(Math.Pow(object1.x - object2.x, 2) + Math.Pow(object2.z - object1.z, 2));
    }
}

