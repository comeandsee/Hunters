using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using System.Linq;
using Mapbox.Utils;

public class AnimalFactory : Singleton<AnimalFactory>
{


    [SerializeField] private Animal[] availableAnimals;
    // [SerializeField] private float waitTime = 180.0f;


    [SerializeField] private List<Animal> liveAnimals = new List<Animal>();

    private List<int> lvlAnimalsIndex = new List<int>();
    [SerializeField] private Animal selectedAnimal;


    private string nameSelectedAnimal = "animal";
    private int pointsSelectedAnimal = 100;

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
            if (availableAnimals[i].Lvl == lvl)
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

        NameSelectedAnimal = animal.name;
        PointsSelectedAnimal = animal.Points;

        selectedAnimal = null;
    }

    public Animal[] AvailableAnimals { get => availableAnimals; set => availableAnimals = value; }
    public List<int> LvlAnimalsIndex { get => lvlAnimalsIndex; set => lvlAnimalsIndex = value; }
    public string NameSelectedAnimal { get => nameSelectedAnimal; set => nameSelectedAnimal = value; }
    public int PointsSelectedAnimal { get => pointsSelectedAnimal; set => pointsSelectedAnimal = value; }

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
            CreateAnimalsOnLvl(lvl: player.Lvl);

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
            //  InstantiateAnimal();

            //on area
            GameAreaCoordinates gameArea = new GameAreaCoordinates();
            InstantiateAnimalsOnArea(gameArea);
        }
        UpdatePlayerValues();


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
        int indexInLvlAnimalsIndex = Random.Range(0, LvlAnimalsIndex.Count);
        int index = LvlAnimalsIndex[indexInLvlAnimalsIndex];

        float x = player.transform.position.x + GenerateRange();
        float z = player.transform.position.z + GenerateRange();
        float y = player.transform.position.y;
        liveAnimals.Add(Instantiate(AvailableAnimals[index], new Vector3(x, y, z), Quaternion.identity));
    }


    private float GenerateRange()
    {
        float randomNum = Random.Range(HuntersConstants.minRange, HuntersConstants.maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return randomNum * (isPositive ? 1 : -1);
    }

    public class Coordinates
    {
        public float lat;
        public float lon;
        public Coordinates(float lat, float lon)
        {
            this.lat = lat;
            this.lon = lon;
        }
    }

    public class GameAreaCoordinates
    {
        public Coordinates northWest;
        public Coordinates northEast;
        public Coordinates southEast;
        public Coordinates southWest;

        public GameAreaCoordinates(Coordinates northWest, Coordinates northEast, Coordinates southEast, Coordinates southWest)
        {
            this.northWest = northWest;
            this.northEast = northEast;
            this.southEast = southEast;
            this.southWest = southWest;
        }

        public GameAreaCoordinates()
        {
            this.northWest = new Coordinates(60.193635f, 24.967477f);
            this.northEast = new Coordinates(60.193736f, 24.971189f);
            this.southEast = new Coordinates(60.192200f, 24.9687445f);
            this.southWest = new Coordinates(60.191901f, 24.970663f);
        }
        public GameAreaCoordinates(bool isGdansk)
        {
            this.northWest = new Coordinates(54.367873f, 18.609933f);
            this.northEast = new Coordinates(54.367857f, 18.612052f);
            this.southEast = new Coordinates(54.366369f, 18.611564f);
            this.southWest = new Coordinates(54.366553f, 18.609472f);
        }
    }

    private void InstantiateAnimalsOnArea(GameAreaCoordinates gameArea)
    {
        int indexInLvlAnimalsIndex = Random.Range(0, LvlAnimalsIndex.Count);
        int index = LvlAnimalsIndex[indexInLvlAnimalsIndex];

        Coordinates randomCoordinates = GenerateRangeCooridantes(gameArea);
        InstancePrefabOnRealMap(AvailableAnimals[index], randomCoordinates.lat, randomCoordinates.lon);
    }

    private Coordinates GenerateRangeCooridantes(GameAreaCoordinates gameArea)
    {
        float[] xArray = { gameArea.northWest.lat, gameArea.northEast.lat, gameArea.southEast.lat, gameArea.southWest.lat };
        float[] yArray = { gameArea.northWest.lon, gameArea.northEast.lon, gameArea.southEast.lon, gameArea.southWest.lon };

        float randomLat = Random.Range(xArray.Min(), xArray.Max());
        float randomLon = Random.Range(yArray.Min(), yArray.Max());

        return new Coordinates(randomLat, randomLon);
    }



    [SerializeField]
    AbstractMap _map;
    float _spawnScale = 100f;

    private Animal InstancePrefabOnRealMap(Animal _markerPrefab, double lat, double lon)
    {

        StartCoroutine(waitUntilMapShowUp(2, _markerPrefab, lat, lon));

        return new Animal();


    }

    private IEnumerator waitUntilMapShowUp(float waitTime, Animal _markerPrefab, double lat, double lon) 
    {
        //    yield return new WaitForSeconds(waitTime);

            Vector2d zeroVector = new Vector2d(0.00000, 0.00000);

            yield return new WaitUntil(() => !_map.CenterMercator.normalized.Equals(zeroVector));
            Animal instance = Instantiate(_markerPrefab);

            liveAnimals.Add(instance);
            UpdatePlayerValues();

            var locations = new Vector2d(lat, lon);
            instance.transform.localPosition = _map.GeoToWorldPosition(locations, true);
            instance.transform.position = _map.GeoToWorldPosition(locations, true);
        
    }

    private void UpdatePlayerValues()
    {
        int maxPoints = 0;
        foreach (var animal in liveAnimals)
        {
            maxPoints += animal.Points;
        }
        player.Xp = 0;
        player.LevelBase = maxPoints;
        player.RequiredXp = maxPoints;
    }
}
