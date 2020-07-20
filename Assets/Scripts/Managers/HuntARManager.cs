﻿using System;
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
      //  GetHuntedAnimal();
    }

    // Update is called once per frame
    void Update()
    {
     //   huntAnimal();
    }

    public void GetHuntedAnimal()
    {
        this.getAnimalWithMinDistance();
    }

    private void getAnimalWithMinDistance()
    {
        var liveAnimals = AnimalFactory.Instance.LiveAnimals;

        Animal animalWithMinDistance = null;
        double minDistance;

        //first animal
        minDistance = calculateDistance(GameObject.FindWithTag("Player"), liveAnimals[0].gameObject);


        //find min distance
        foreach (Animal liveAnimal in liveAnimals)
        {
            var distance = calculateDistance(GameObject.FindWithTag("Player"), liveAnimal.gameObject);

            if (distance <= minDistance)
            {
                animalWithMinDistance = liveAnimal;
                minDistance = distance;
            }
        }

        huntedAnimal = animalWithMinDistance;

        AnimalFactory.Instance.SelectedAnimal = huntedAnimal;

        var distanceState = updateDistanceStatus(minDistance);
        var uI = FindObjectOfType<UIManager>();
        uI.updateDistanceStatusUI(distanceState);
    }

   
    private void huntAnimal()
    {
        if (areTracks)
        {
            var distanceState = UpdateHuntingAnimalDistance(huntedAnimal);

            //slady tutaj dodaj
            AnimalFactory.Instance.createTracksByPlayerPosition();

         //   areTracks = false;
        }

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
