using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class AnimalFactory : Singleton<AnimalFactory>
{


    [SerializeField] private Animal[] availableAnimals;
   // [SerializeField] private float waitTime = 180.0f;


    [SerializeField] private List<Animal> liveAnimals = new List<Animal>();

    private List<int> lvlAnimalsIndex = new List<int>();
    [SerializeField] private Animal selectedAnimal;
    private Player player;


    public void CreateAnimalsOnLvl(int lvl)
    {
        LvlAnimalsIndex = new List<int>();

        for (int i = 0; i < liveAnimals.Count; i++)
        {
            liveAnimals[i].deleteMe();
        }
        liveAnimals = new List<Animal>();


        for (int i = 0; i < availableAnimals.Length; i++)
        {
            if(availableAnimals[i].Lvl == lvl)
            {
                LvlAnimalsIndex.Add(i);
            }
        }

        createAnimals();
    }

    public List<Animal> LiveAnimals
    {
        get { return liveAnimals; }
    }

    public Animal SelectedAnimal
    {
        get { return selectedAnimal; }
    }

    public void gatherAnimal(Animal animal)
    {
        player.AddXp(animal.Points);
        liveAnimals.Remove(animal);
        selectedAnimal = null;
    }

    public Animal[] AvailableAnimals { get => availableAnimals; set => availableAnimals = value; }
    public List<int> LvlAnimalsIndex { get => lvlAnimalsIndex; set => lvlAnimalsIndex = value; }

    // public ListDictionary LiveAnimalsWithIndex { get => liveAnimalsWithIndex; set => liveAnimalsWithIndex = value; }

    private void Awake()
    {
      //  DontDestroyOnLoad(this);
        Assert.IsNotNull(AvailableAnimals);
        player = GameManager.Instance.CurrentPlayer;
        Assert.IsNotNull(player);
       
    }

    void Start()
    {
        var animalsFactories = FindObjectsOfType<AnimalFactory>();

         if (animalsFactories.Length <= 1)
        {
            CreateAnimalsOnLvl(lvl:player.Lvl);
        }

        /*
        if (player)
        {
            foreach (Animal animal in liveAnimals)
            {
                player.AddAnimal(animal.gameObject);
            }
        }*/
        // StartCoroutine(GenerateAnimals());
    }


    private void createAnimals()
    {
        for (int i = 0; i < HuntersConstants.startingAnimals; i++)
        {
            InstantiateAnimal();
        }
        int maxPoints = 0;
        foreach (var animal in liveAnimals)
        {
            maxPoints += animal.Points;
        }
        player.Xp = 0;
        player.LevelBase = maxPoints;
        player.RequiredXp = maxPoints;


    }
    public void AnimalWasSelected(Animal Animal)
    {
        selectedAnimal = Animal;
    }

    private IEnumerator GeneratePlants()
    {
        while (true)
        {
            // InstantiatePlant();
          //  yield return new WaitForSeconds(waitTime);
        }
    }

    private void InstantiateAnimal()
    {
        //for (int i = 0; i < AvailableAnimals.Length; i++)
      //  {


              int indexInLvlAnimalsIndex = Random.Range(0, LvlAnimalsIndex.Count);
             int index = LvlAnimalsIndex[indexInLvlAnimalsIndex];
            //int index = i;
            float x = player.transform.position.x + GenerateRange();
            float z = player.transform.position.z + GenerateRange();
            float y = player.transform.position.y;
            liveAnimals.Add(Instantiate(AvailableAnimals[index], new Vector3(x, y, z), Quaternion.identity));
      //  }
    }

    private float GenerateRange()
    {
        float randomNum = Random.Range(HuntersConstants.minRange, HuntersConstants.maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return randomNum * (isPositive ? 1 : -1);
    }

     
}
