using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimalData 
{
    private float spawnRate;
    private float catchRate ;
    private int attack ;
    private int defense ;
    private String animalSound;
    private int hp;

//    private int indexInFactory;

    public float SpawnRate { get => spawnRate;  }
    public float CatchRate { get => catchRate;  }
    public int Attack { get => attack;  }
    public int Defense { get => defense;  }
    public String AnimalSound { get => animalSound;  }
    public int Hp { get => hp; }
   // public int IndexInFactory { get => indexInFactory;  }

    public AnimalData(Animal animal)
    {
        spawnRate = animal.SpawnRate;
        catchRate = animal.CatchRate;
        attack = animal.Attack;
        defense = animal.Defense;
        animalSound = animal.AnimalSound.name;//AudioSource.name;
        hp = animal.Hp;

    
    //    indexInFactory = AnimalFactory.Instance.AvailableAnimals;

    }


 
}
