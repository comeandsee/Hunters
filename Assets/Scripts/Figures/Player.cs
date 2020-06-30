using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] private int xp = 0;
	[SerializeField] private int requiredXp = 100;
	[SerializeField] private int levelBase = 200;
	[SerializeField] private List<GameObject> animals = new List<GameObject>();

    private bool newLvl = false;
    private bool endGame = false;

    private int lvl = 1;
	private string path;

	public int Xp {
		get { return xp; }
        set { xp = value; }
	}

	public int RequiredXp {
		get { return requiredXp; }
        set { requiredXp = value; }
    }

	public  int LevelBase {
		get { return levelBase; }
        set { levelBase = value; }
    }

	public List<GameObject> Animals {
		get { return animals; }
	}

	public int Lvl {
		get { return lvl; }
	}

    public bool NewLvl { get => newLvl; set => newLvl = value; }
    public bool EndGame { get => endGame; set => endGame = value; }

    private void Awake()
    {
        path = Application.persistentDataPath + "/player.dat";
        Load();
    }
    private void Start() {	
	}

	public void AddXp(int xp) {
		this.xp += Mathf.Max(0, xp);
		InitLevelData();
		Save();
	}

	public void AddAnimal(GameObject animal) {
		if (animal)
           animals.Add(animal);
        Save();
	}

	private void InitLevelData() {
        int lvlBefore = lvl;
		lvl = (xp / levelBase) + lvl;

        if(lvl >= HuntersConstants.maxLvl)
        {
            EndGame = true;
        }
        else if (lvlBefore < lvl) 
        {
            AnimalFactory.Instance.CreateAnimalsOnLvl(lvl);
            NewLvl = true;
        }

        requiredXp = levelBase;//* lvl;
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

			
            lvl = data.Lvl;
            /*
            foreach (AnimalData animalData in data.Animals)
            {
                GameObject animalObj = new GameObject();
                Animal animal = null;
                Animal rodzaj = AnimalFactory.Instance.AvailableAnimals[animalData.IndexInFactory];
               // var typeAnimal = ;
               // animal = animalObj.AddComponent<(rodzaj.GetType())>();
               // Array.IndexOf(, animal);
                //var animal = animalObj.AddComponent<Animal>();
                animal.loadFromAnimalData(animalData);
                AddAnimal(animal.gameObject);
             
            }
            */
            /*
            foreach (AnimalData animalData in data.Animals)
            {
                
                Animal animal = new Animal() ;
                animal.SpawnRate= animalData.SpawnRate;
                animal.CatchRate = animalData.CatchRate;
                animal.Attack = animalData.Attack;
                animal.Defense = animalData.Defense;
                animal.AudioSource.name = animalData.AnimalSound;
                animal.Hp = animalData.Hp;

                 GameObject gameObject = animal.GetComponent<GameObject>();
                 if (gameObject != null)
                 {
                    animals.Add(animal);
                 }
            }*/
        }
		else {
			InitLevelData();
		}
		
	}


    public void startFromBeginning()
    {
        endGame = false;
        newLvl = false;
        lvl = 1;
        xp = 0;
        requiredXp = 100;
        levelBase = 200;
        animals = new List<GameObject>();

        Save();
        AnimalFactory.Instance.CreateAnimalsOnLvl(lvl);
    }

}



