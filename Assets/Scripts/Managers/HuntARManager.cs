using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HuntersConstants;

public class HuntARManager : Singleton<HuntARManager>
{
    private bool searchForNewAnimal = true;
    private bool areTracks = true;

    private Animal huntedAnimal;



    // Start is called before the first frame update
    void Start()
    {
      //  FindHuntedAnimal();
    }

  
    void Update()
    {
        //huntAnimal();
    }

    private void FindHuntedAnimal()
    {
        //get nearest animal 
        this.getHuntedAnimalAndUpdateUI();
    }

    private void getHuntedAnimalAndUpdateUI()
    {
        double minDistance;
        huntedAnimal = GetHuntedAnimal(out minDistance);

        AnimalFactory.Instance.SelectedAnimal = huntedAnimal;

        var distanceState = updateDistanceStatus(minDistance);
        var uI = FindObjectOfType<UIManager>();
        uI.updateDistanceStatusUI(distanceState);
    }

    public Animal GetHuntedAnimal( out double minDistance)
    {
        var liveAnimals = AnimalFactory.Instance.AnimalsInstances;

        Animal animalWithMinDistance = null;

        //first animal
        minDistance = calculateDistance(GameObject.FindWithTag("Player"), liveAnimals[0].Animal.gameObject);


        //find min distance
        foreach (var liveAnimal in liveAnimals)
        {
            var distance = calculateDistance(GameObject.FindWithTag("Player"), liveAnimal.Animal.gameObject);

            if (distance <= minDistance)
            {
                animalWithMinDistance = liveAnimal.Animal;
                minDistance = distance;
            }
        }

        return animalWithMinDistance;
    }

    private void huntAnimal()
    {
        var distanceState = UpdateHuntingAnimalDistance(huntedAnimal);

        AnimalFactory.Instance.createTracksByPlayerPosition();

        //usuwanie sladow
        //todo
    }

    private distanceZone UpdateHuntingAnimalDistance(Animal searchingAnimal)
    {
        var updatedDistance = calculateDistance(GameObject.FindWithTag("Player"), searchingAnimal.gameObject);

        //update status
        var distanceState = updateDistanceStatus(updatedDistance);
        var uI = FindObjectOfType<UIManager>();
        uI.updateDistanceStatusUI(distanceState);

        return distanceState;
    }

    private double calculateDistance(GameObject object1, GameObject object2)
    {
        var heading = object1.transform.position - object2.transform.position;
        return heading.magnitude;
    }


    private Vector3 getPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        return player.transform.position;
    }


    public distanceZone updateDistanceStatus(double updatedDistance)
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

}
