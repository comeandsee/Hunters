using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{

    private int lvl;


    public int Lvl { get => lvl;  }

    public PlayerData(Player player) {

        lvl = player.Lvl;


     /*  foreach (GameObject animalObject in player.Animals)
        {
            Animal animal = animalObject.GetComponent<Animal>();
            if(animal != null)
            {
                AnimalData data = new AnimalData(animal);
                animals.Add(data);
            }

        }
        */
     

    }
}
