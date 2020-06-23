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

    private AudioSource audioSource;

    public float SpawnRate { get => spawnRate; set => spawnRate = value; }
    public float CatchRate { get => catchRate; set => catchRate = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Defense { get => defense; set => defense = value; }
    public String AnimalSound { get => animalSound; set => animalSound = value; }
    public int Hp { get => hp; set => hp = value; }

    public AnimalData(Animal animal)
    {
        spawnRate = animal.SpawnRate;
        catchRate = animal.CatchRate;
        attack = animal.Attack;
        defense = animal.Defense;
        animalSound = animal.AudioSource.name;
        hp = animal.Hp;
    }
}
