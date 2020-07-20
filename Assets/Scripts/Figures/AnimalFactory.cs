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
using static Location;
using static HuntersConstants;

public class AnimalFactory : Singleton<AnimalFactory>
{
    [SerializeField] private Animal[] availableAnimals;
    [SerializeField] private GameObject[] availableFootsteps;
    [SerializeField] private List<Animal> liveAnimals = new List<Animal>();
    [SerializeField] private List<GameObject> liveAnimalsFootsteps = new List<GameObject>();

    [SerializeField] AbstractMap _map;
 
    [SerializeField] private Animal selectedAnimal;

    private List<int> lvlAnimalsIndex = new List<int>();
    private string nameSelectedAnimal = "animal";
    private int pointsSelectedAnimal = 100;
    private bool tracksDelay = false;
    private Player player;
    private float resetDelayTime = 20f;


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
        set { selectedAnimal = value; }
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
    public List<GameObject> LiveAnimalsFootsteps { get => liveAnimalsFootsteps; set => liveAnimalsFootsteps = value; }
    public GameObject[] AvailableFootsteps { get => availableFootsteps; set => availableFootsteps = value; }

    public void AnimalWasSelected(Animal Animal)
    {
        selectedAnimal = Animal;
    }

    private void Awake()
    {
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


    private void InstancePrefabOnRealMap(Animal _markerPrefab, double lat, double lon)
    {
        StartCoroutine(waitUntilMapShowUpAndCreateAnimalsOnMap(2, _markerPrefab, lat, lon));
    }

    private IEnumerator waitUntilMapShowUpAndCreateAnimalsOnMap(float waitTime, Animal _markerPrefab, double lat, double lon) 
    {
            Vector2d zeroVector = new Vector2d(0.00000, 0.00000);
            yield return new WaitUntil(() => !_map.CenterMercator.normalized.Equals(zeroVector));
            Animal instance = Instantiate(_markerPrefab);

            liveAnimals.Add(instance);
            UpdatePlayerValues();

            var locations = new Vector2d(lat, lon);
            instance.transform.localPosition = _map.GeoToWorldPosition(locations, true);
            instance.transform.position = _map.GeoToWorldPosition(locations, true);

            createTrack(instance);


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

    public void createTracksByPlayerPosition()
    {
        if (!tracksDelay)
        {
            var playerPosition = getPlayerPosition();

            //position of target
            var heading = selectedAnimal.transform.position - playerPosition;
            var distance = heading.magnitude;
            var direction = heading / distance;

            heading.y = 0;

            var position = playerPosition + direction;

            liveAnimalsFootsteps.Add(Instantiate(AvailableFootsteps[0], position, Quaternion.identity));
            liveAnimalsFootsteps.Last().transform.rotation = Quaternion.LookRotation(direction);

            tracksDelay = true;
            StartCoroutine(resetDelay());
        }
    }


    private Vector3 getPlayerPosition()
    {
        var player = GameObject.FindWithTag("Player");
        return player.transform.position;
    }

    IEnumerator resetDelay()
    {
        yield return new WaitForSeconds(resetDelayTime);
        tracksDelay = false;
    }


    private void createTrack(Animal animal )
    {
        if (liveAnimals.Count <= 1)
        {
            var tracksNumber = Random.Range(10, 10);
            var step = 0;

           var xPositive = isPositve();
           var yPositive = isPositve();

            GameObject prevFootstep = null;

            for (int i = 0; i < tracksNumber; i++)
            {

                var basic = (float)distanceZone.close + step;
                var distanceX = Random.Range(basic, basic + (float)distanceZone.middle) *  xPositive;
                var distanceZ = Random.Range(basic, basic + (float)distanceZone.middle) *  yPositive;

                var distance = new Vector3(distanceX, 0, distanceZ);

                var position = animal.transform.position  + distance;
                var footstep = Instantiate(AvailableFootsteps[0], position, Quaternion.identity);



                //roration of track
                if (!prevFootstep) { prevFootstep = animal.gameObject; };

                var headingAnimal = prevFootstep.transform.position - footstep.transform.position;

                var distanceFromAnimal = headingAnimal.magnitude;
                var directionToPrevFootstep = headingAnimal / distanceFromAnimal;

                footstep.transform.rotation = Quaternion.LookRotation(directionToPrevFootstep);

                prevFootstep = footstep;
                step += 10;
            }
           

        }

    }

    private int isPositve()
    {
        bool isPositive = Random.Range(0, 10) < 5;
        return (isPositive ? 1 : -1);
    }

    private int isPositveNumber(float number )
    {
        bool isPositive = number >= 0;
        return (isPositive ? 1 : -1);
    }
}
