using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour {

	[SerializeField] private int xp = 0;
	[SerializeField] private int requiredXp = 100;
	[SerializeField] private int levelBase = 200;
	[SerializeField] private List<GameObject> animals = new List<GameObject>();
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject camAR;

    private bool newLvl = false;
    private bool endGame = false;

    private int lvl = 1;
	private string path;

    [SerializeField] public float Timer = 0.0f;
    private float addPointTime = 60.0f;
    private float lastUpdate = 0.0f;
    private int bonusXP = 1;
    private Vector3 lastPosition = new Vector3(0, 0, 0);

    [SerializeField] public float TimerRotation = 0.0f;
    private float RotationTime = 10.0f;
    private float lastUpdateRotation = 0.0f;


    public void Update()
    {
        Timer += Time.deltaTime;
        if(Time.time - lastUpdate >= addPointTime)
        {
            bonusXP = (int)(0.01 * requiredXp);
            AddXp(bonusXP);


            lastUpdate = Time.time;
        }


        TimerRotation += Time.deltaTime;
        if (Time.time - lastUpdateRotation >= RotationTime)
        {
            var direction = getDirection();
            lastPosition = transform.position;
            lastUpdateRotation = Time.time;
        }

        // rotateInDirectionOnMove();


    }

    public float speed = 1.0f;
    public void rotateInDirectionOnMove()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = lastPosition - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
        playerBody.transform.rotation = Quaternion.LookRotation(newDirection); 
    }
    public Vector3 getDirection()
    {
        var heading =  this.transform.position - lastPosition ;
        var distance= heading.magnitude;
        Vector3 direction;
        if (distance != 0) direction = heading / distance;
        else direction = heading;



            // direction = direction;

            Debug.DrawRay(transform.position, direction, Color.red);
        Debug.DrawRay(transform.position, direction + heading, Color.red, 20);
        //      transform.rotation = Quaternion.LookRotation(direction);
        //   transform.Translate(direction * 10 * Time.deltaTime, Space.Self);
        // transform.Rotate(direction);
        //   transform.Rotate(0, 1 * Time.deltaTime, 0, Space.Self); //rotate
        //   transform.Translate(1 * Time.deltaTime, 0, 0, Space.World);

        if (playerBody != null)
        {
            var a = playerBody.gameObject.transform.localRotation;
            var b = Quaternion.LookRotation(direction);
 
           // var c = Time.deltaTime * 40f;
            if (b != new Quaternion(0, 0, 0, 1))
            {
                playerBody.gameObject.transform.rotation = Quaternion.Slerp(a, b, 1);

                if (camAR != null)
                {
                    camAR.gameObject.transform.rotation = Quaternion.Slerp(a, b, 1);
                }

            }
        }
        //

       // transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, direction, 40 * Time.deltaTime, 0.0F));

        //  this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15F);
        return direction;
    }
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
        set { lvl = value; }
    }

    public bool NewLvl { get => newLvl; set => newLvl = value; }
    public bool EndGame { get => endGame; set => endGame = value; }

    private void Awake()
    {
        path = Application.persistentDataPath + "/player.dat";
        Load();
    }
    private void Start() {
        Load();
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


    public void startFromBeginning(int lvl = 1)
    {
        EndGame = false;
        NewLvl = false;
        Lvl = lvl;
        xp = 0;
        requiredXp = 100;
        levelBase = 200;
        animals = new List<GameObject>();

        Save();
        AnimalFactory.Instance.CreateAnimalsOnLvl(lvl);
    }



}



