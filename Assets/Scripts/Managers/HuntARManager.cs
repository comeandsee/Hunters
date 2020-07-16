using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HuntersConstants;

public class HuntARManager : Singleton<HuntARManager>
{
    private bool searchForNewAnimal = true;

    private Animal huntedAnimal;



    // Start is called before the first frame update
    void Start()
    {
        GetHuntedAnimal();
    }

    // Update is called once per frame
    void Update()
    {
        huntAnimal();
    }

    public void GetHuntedAnimal()
    {
        this.getAnimalWithMinDistance();
    }

    private void getAnimalWithMinDistance()
    {
        var playerPosition = getPlayerPosition();
        var liveAnimals = AnimalFactory.Instance.LiveAnimals;

        Animal animalWithMinDistance = null;
        double minDistance;

        //first animal
        var animalPosition = liveAnimals[0].transform.position;
        minDistance = calculateDistance(playerPosition, animalPosition);


        //find min distance
        foreach (Animal liveAnimal in liveAnimals)
        {
            animalPosition = liveAnimal.transform.position;

            var distance = calculateDistance(playerPosition, animalPosition);

            if (distance <= minDistance)
            {
                animalWithMinDistance = liveAnimal;
                minDistance = distance;
            }
        }

        huntedAnimal = animalWithMinDistance;

        var distanceState = updateDistanceStatus(minDistance);
        var uI = FindObjectOfType<UIManager>();
        uI.updateDistanceStatusUI(distanceState);
    }

   
    private void huntAnimal()
    {
        UpdateHuntingAnimalDistance(huntedAnimal);
  
    }

    private void UpdateHuntingAnimalDistance(Animal searchingAnimal)
    {
        var playerPosition = getPlayerPosition();

        var updatedDistance = calculateDistance(playerPosition, searchingAnimal.transform.position);

        //update status
        var distanceState = updateDistanceStatus(updatedDistance);
        var uI = FindObjectOfType<UIManager>();
        uI.updateDistanceStatusUI(distanceState);
    }

    private double calculateDistance(Vector3 object1, Vector3 object2)
    {
        return Math.Sqrt(Math.Pow(object1.x - object2.x, 2) + Math.Pow(object2.z - object1.z, 2));
    }


    private Vector3 getPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        return player.transform.position;
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

}
