using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimalData 
{

    private String animalSound;
    private int hp;
    private int points;
    private int lvl ;


    public String AnimalSound { get => animalSound;  }
    public int Hp { get => hp; }
    public int Points { get => points; }
    public int Lvl { get => lvl; }

    // public int IndexInFactory { get => indexInFactory;  }

    public AnimalData(Animal animal)
    {
        animalSound = animal.AnimalSound.name;//AudioSource.name;
        hp = animal.Hp;
        points = animal.Points;
        lvl = animal.Lvl;
    
    //    indexInFactory = AnimalFactory.Instance.AvailableAnimals;

    }


 
}
