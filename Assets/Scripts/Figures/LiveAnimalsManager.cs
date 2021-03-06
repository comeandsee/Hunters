﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static HuntersConstants;

public class LiveAnimalsManager : Singleton<LiveAnimalsManager>
{
    private bool enabled = true; 
    private bool disabled = false;
    private float distance = distanceToShowAnimal;
    private float distanceFootsteps = distanceToShowFootsteps;


    private void Update()
    {
        var uI = FindObjectOfType<UIManager>();

        if (HuntersConstants.isDebugMode)
        {
            enabled = true;
            disabled = true;
            uI.clearDebugTxt();
        }
        else
        {
            enabled = true;
            disabled = false;
        }

        var playerPosition = getPlayerPosition();
        var liveAnimals =  AnimalFactory.Instance.AnimalsInstances;

        foreach (var animal in liveAnimals)
        {
            var animalPlayerDistance = GetDistance(playerPosition, animal.Animal.gameObject);

            uI.addDebugTxt(animal.Animal.name.Replace("(Clone)","") + ": " + animalPlayerDistance.ToString("0.00"));

            if (animalPlayerDistance <= distance)showAnimal(animal.Animal, enabled);
            else showAnimal(animal.Animal, disabled);

            foreach (var footstep in animal.Footsteps)
            {
                if (GetDistance(playerPosition, footstep.gameObject) <= distanceFootsteps) showFootprint(footstep, enabled);
                else showFootprint(footstep, disabled);
            }
        }
    }

     private IEnumerator waitToMapLoadAndDisabledAllAnimals()
    {
        yield return new WaitUntil(() => AnimalFactory.Instance.AnimalsInstances.ToArray().Length == HuntersConstants.startingAnimals);

     //   showAllAnimals(true);
       // showAllFootprints(true);
    }

    private  float GetDistance(Vector3 playerPosition, GameObject gameObject)
    {
        var heading = gameObject.transform.position - playerPosition;
        return heading.magnitude;
    }

    public void showAllAnimals(bool enabled = true)
    {
        List<LiveAnimal> animalsInstances = AnimalFactory.Instance.AnimalsInstances;
        foreach (var animal in animalsInstances)
        {
            animal.Animal.gameObject.SetActive(enabled);
        }

 
    }

    public void showAnimal(Animal animal,bool enabled = true)
    {
        animal.gameObject.SetActive(enabled);
    }

    public void showAllFootprints(bool enabled = true)
    {
        List<LiveAnimal> animalsInstances = AnimalFactory.Instance.AnimalsInstances;
        foreach (var animal in animalsInstances)
        {
            foreach (var footstep in animal.Footsteps)
            {
                footstep.gameObject.SetActive(enabled);
            }
        }
    }


    public void showFootprint(GameObject footprint, bool enabled = true)
    {
        /*   List<LiveAnimal> animalsInstances = AnimalFactory.Instance.AnimalsInstances;
           var indexInAnimalsInstances = animalsInstances.IndexOf(animalsInstances.Find(a => a.Footsteps.Find( f => f == footprint)));

           animalsInstances[indexInAnimalsInstances].Footsteps.Find(f => f == footprint).SetActive(false);
           */
        footprint.gameObject.SetActive(enabled);
   }

    private Vector3 getPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        return player.transform.position;
    }
}
