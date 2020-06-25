using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class AnimalFactory : Singleton<AnimalFactory>
{


    [SerializeField] private Animal[] availableAnimals;
    [SerializeField] private float waitTime = 180.0f;
    [SerializeField] private int startingAnimals = 10;
    [SerializeField] private float minRange = 5.0f;
    [SerializeField] private float maxRange = 20.0f;

    [SerializeField] private List<Animal> liveAnimals = new List<Animal>();
    //  private ListDictionary liveAnimalsWithIndex = new ListDictionary();
    [SerializeField] private Animal selectedAnimal;
    private Player player;

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
        liveAnimals.Remove(animal);
        selectedAnimal = null;
    }

    public Animal[] AvailableAnimals { get => availableAnimals; set => availableAnimals = value; }
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
        
           var a = FindObjectOfType<AnimalFactory>();
           var b = FindObjectsOfType<AnimalFactory>();

        if (b.Length <= 1)
        {
            createAnimals();

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
        for (int i = 0; i < startingAnimals; i++)
        {
            InstantiateAnimal();
        }

    }
    public void AnimalWasSelected(Animal Animal)
    {
        selectedAnimal = Animal;
    }

    private IEnumerator GenerateAnimals()
    {
        while (true)
        {
            InstantiateAnimal();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void InstantiateAnimal()
    {
        int index = Random.Range(0, AvailableAnimals.Length);
        float x = player.transform.position.x + GenerateRange();
        float z = player.transform.position.z + GenerateRange();
        float y = player.transform.position.y;
        liveAnimals.Add(Instantiate(AvailableAnimals[index], new Vector3(x, y, z), Quaternion.identity));
      //  LiveAnimalsWithIndex.Add(index, liveAnimals[index]);
    }

    private float GenerateRange()
    {
        float randomNum = Random.Range(minRange, maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return randomNum * (isPositive ? 1 : -1);
    }

     
}
