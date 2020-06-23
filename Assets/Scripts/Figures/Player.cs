using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] private int xp = 0;
	[SerializeField] private int requiredXp = 100;
	[SerializeField] private int levelBase = 100;
	[SerializeField] private List<Animal> animals = new List<Animal>();
	
	private int lvl = 1;
	private string path;

	public int Xp {
		get { return xp; }
	}

	public int RequiredXp {
		get { return requiredXp; }
	}

	public int LevelBase {
		get { return levelBase; }
	}

	public List<Animal> Animals {
		get { return animals; }
	}

	public int Lvl {
		get { return lvl; }
	}
	
	private void Start() {
        //InitLevelData();
		path = Application.persistentDataPath + "/player.dat";
		Load();
	}

	public void AddXp(int xp) {
		this.xp += Mathf.Max(0, xp);
		InitLevelData();
		Save();
	}

	public void AddAnimal(Animal animal) {
		if (animal)
           animals.Add(animal);
        Save();
	}

	private void InitLevelData() {
		lvl = (xp / levelBase) + 1;
		requiredXp = levelBase * lvl;
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(path);
		PlayerData data = new PlayerData(this);
		bf.Serialize(file, data);
		file.Close();
	}

	private void Load() {
		if (File.Exists(path)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);
			PlayerData data = (PlayerData) bf.Deserialize(file);
			file.Close();

			xp = data.Xp;
			requiredXp = data.RequiredXp;
			levelBase = data.LvlBase;
            lvl = data.Lvl;


            foreach (AnimalData animalData in data.Animals)
            {
                Animal animal = new Animal() ;
                 animal.SpawnRate= animalData.SpawnRate;
                 animal.CatchRate = animalData.CatchRate;
                animal.Attack = animalData.Attack;
                animal.Defense = animalData.Defense;
                animal.AudioSource.name = animalData.AnimalSound;
               animal.Hp = animalData.Hp;
                
               
               animals.Add(animal);
            }
		}
		else {
			InitLevelData();
		}
		
	}
}



