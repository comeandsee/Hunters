using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class AnimalFactory : Singleton<AnimalFactory>
{


    [SerializeField] private Animal[] availableAnimals;
    [SerializeField] private float waitTime = 180.0f;
    [SerializeField] private int startingAnimals = 5;
    [SerializeField] private float minRange = 1.0f;
    [SerializeField] private float maxRange = 30.0f;

    private List<Animal> liveAnimals = new List<Animal>();
    private Animal selectedAnimal;
    [SerializeField]private Player player;

    public List<Animal> LiveAnimals
    {
        get { return liveAnimals; }
    }

    public Animal SelectedAnimal
    {
        get { return selectedAnimal; }
    }

    private void Awake()
    {
        Assert.IsNotNull(availableAnimals);
        Assert.IsNotNull(player);
    }

    void Start()
    {
        //player = GameManager.Instance.CurrentPlayer;
        //Assert.IsNotNull(player);
        for (int i = 0; i < startingAnimals; i++)
        {
            InstantiateAnimal();
             
        }

        StartCoroutine(GenerateAnimals());
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
        int index = Random.Range(0, availableAnimals.Length);
        float x = player.transform.position.x + GenerateRange();
        float z = player.transform.position.z + GenerateRange();
        float y = player.transform.position.y;
        liveAnimals.Add(Instantiate(availableAnimals[index], new Vector3(x, y, z), Quaternion.identity));
    }

    private float GenerateRange()
    {
        float randomNum = Random.Range(minRange, maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return randomNum * (isPositive ? 1 : -1);
    }
}
