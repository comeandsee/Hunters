using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{
    private int xp;
    private int requiredXp;
    private int lvlBase;
    private int lvl;
    private List<AnimalData> animals;

    public int Xp{ get => xp; }

    public int RequiredXp { get => requiredXp;  }
    public int LvlBase { get => lvlBase;  }
    public int Lvl { get => lvl;  }
    public List<AnimalData> Animals { get { return animals; } }

    public PlayerData(Player player) {
        xp = player.Xp;
        requiredXp = player.RequiredXp;
        lvlBase = player.LevelBase;
        lvl = player.Lvl;
       foreach (Animal animalObject in player.Animals)
        {
            Animal animal = animalObject.GetComponent<Animal>();
            if(animal != null)
            {
                AnimalData data = new AnimalData(animal);
                animals.Add(data);
            }

        }

    }
}
