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
    // [SerializeField] private List<Animal> liveAnimals = new List<Animal>();
    //[SerializeField] private List<GameObject> liveAnimalsFootsteps = new List<GameObject>();
    [SerializeField] private List<LiveAnimal> animalsInstances = new List<LiveAnimal>();

    [SerializeField] private List<GameObject> footstepsToManualSearch = new List<GameObject>();
    [SerializeField] AbstractMap _map;

    [SerializeField] private Animal selectedAnimal;

    private List<int> lvlAnimalsIndex = new List<int>();
    private string nameSelectedAnimal = "animal";
    private int pointsSelectedAnimal = 100;
    private bool tracksDelay = false;
    private Player player;
    private float resetDelayTime = 20f;

    public bool isMapLoad = false;


    public void CreateAnimalsOnLvl(int lvl)
    {
        LvlAnimalsIndex = new List<int>();

        for (int i = 0; i < animalsInstances.Count; i++)
        {
            animalsInstances[i].destroyAllFootsteps();
            animalsInstances[i].Animal.deleteMe();
        }
        animalsInstances = new List<LiveAnimal>();


        for (int i = 0; i < availableAnimals.Length; i++)
        {
            if (availableAnimals[i].Lvl == lvl)
            {
                LvlAnimalsIndex.Add(i);
            }
        }

        createAnimals();
    }

   

    public Animal SelectedAnimal
    {
        get { return selectedAnimal; }
        set { selectedAnimal = value; }
    }

    public void gatherAnimal(Animal animal)
    {
        player.AddXp(animal.Points);


        var index = animalsInstances.IndexOf(animalsInstances.Find(a => a.Animal == animal));
        animalsInstances[index].destroyAllFootsteps();

        animalsInstances.RemoveAt(index);

        NameSelectedAnimal = animal.name;
        PointsSelectedAnimal = animal.Points;

        selectedAnimal = null;

        var uI = FindObjectOfType<UIManager>();
        uI.ShowSuccessBox() ;
    }

    public Animal[] AvailableAnimals { get => availableAnimals; set => availableAnimals = value; }
    public List<int> LvlAnimalsIndex { get => lvlAnimalsIndex; set => lvlAnimalsIndex = value; }
    public string NameSelectedAnimal { get => nameSelectedAnimal; set => nameSelectedAnimal = value; }
    public int PointsSelectedAnimal { get => pointsSelectedAnimal; set => pointsSelectedAnimal = value; }
   // public List<GameObject> LiveAnimalsFootsteps { get => liveAnimalsFootsteps; set => liveAnimalsFootsteps = value; }
    public GameObject[] AvailableFootsteps { get => availableFootsteps; set => availableFootsteps = value; }
    public List<LiveAnimal> AnimalsInstances { get => animalsInstances; }

    public void AnimalWasSelected(Animal Animal)
    {
        selectedAnimal = Animal;
    }

    private void Awake()
    {
        Assert.IsNotNull(AvailableAnimals);
        player = GameManager.Instance.CurrentPlayer;
        Assert.IsNotNull(player);

        StartCoroutine(waitToMapLoad());
    }

    void Start()
    {
        var animalsFactories = FindObjectsOfType<AnimalFactory>();

        if (animalsFactories.Length <= 1)
        {
            CreateAnimalsOnLvl(lvl: player.Lvl);
        }

      //  StartCoroutine( waitAndGo());
    }


    private void createAnimals()
    {
        for (int i = 0; i < HuntersConstants.startingAnimals; i++)
        {
            if (HuntersConstants.isLocalGame)
            {
                InstantiateAnimal(i);
            }
            else
            {
                //on specific area
                GameAreaCoordinates gameArea = new GameAreaCoordinates(HuntersConstants.isGdansk);
                int indexInAvaiableAnimals = LvlAnimalsIndex[i];
                InstantiateAnimalsOnArea(gameArea, indexInAvaiableAnimals);
            }

          
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

    private void InstantiateAnimal(int indexInLvlAnimalsIndex)
    {
        //int indexInLvlAnimalsIndex = Random.Range(0, LvlAnimalsIndex.Count);
        int index = LvlAnimalsIndex[indexInLvlAnimalsIndex];

        var player = GameObject.FindWithTag("Player");
        var playerPosition = player.transform.position;

        float x = playerPosition.x + GenerateRange();
        float z = playerPosition.z + GenerateRange();
        float y = playerPosition.y;
        animalsInstances.Add( new LiveAnimal (Instantiate(AvailableAnimals[index], new Vector3(x, y, z), Quaternion.identity)));

        var instanceInList = animalsInstances.Last();
        createTrack(instanceInList.Animal, instanceInList);
    }


    private float GenerateRange()
    {
        float randomNum = Random.Range(HuntersConstants.minRange, HuntersConstants.maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return randomNum * (isPositive ? 1 : -1);
    }



    private void InstantiateAnimalsOnArea(GameAreaCoordinates gameArea, int indexInAvaiableAnimals)
    {
        // int indexInLvlAnimalsIndex = Random.Range(0, LvlAnimalsIndex.Count);
        //  int index = LvlAnimalsIndex[indexInLvlAnimalsIndex];     
        Coordinates randomCoordinates = GenerateRangeCooridantes(gameArea);
        InstancePrefabOnRealMap(AvailableAnimals[indexInAvaiableAnimals], randomCoordinates.lat, randomCoordinates.lon);

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
        StartCoroutine(waitUntilMapShowUpAndCreateAnimalsOnMap(_markerPrefab, lat, lon));
    }

    private IEnumerator waitUntilMapShowUpAndCreateAnimalsOnMap(Animal _markerPrefab, double lat, double lon)
    {
        Vector2d zeroVector = new Vector2d(0.00000, 0.00000);
        yield return new WaitUntil(() => !_map.CenterMercator.normalized.Equals(zeroVector));
        Animal instance = Instantiate(_markerPrefab);

        animalsInstances.Add(new LiveAnimal(instance));
        var instanceInList = animalsInstances.Last();
        UpdatePlayerValues();

        var locations = new Vector2d(lat, lon);
        instance.transform.localPosition = _map.GeoToWorldPosition(locations, true);
        instance.transform.position = _map.GeoToWorldPosition(locations, true);

        createTrack(instance , instanceInList);


    }

    private void UpdatePlayerValues()
    {
        int maxPoints = 0;
        foreach (var liveAnimal in animalsInstances)
        {
            maxPoints += liveAnimal.Animal.Points;
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

            footstepsToManualSearch.Add(Instantiate(AvailableFootsteps[0], position, Quaternion.identity));
            footstepsToManualSearch.Last().transform.rotation = Quaternion.LookRotation(direction);

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


    private void createTrack(Animal animal, LiveAnimal instanceInList)
    {

        var tracksNumber = Random.Range(2, 5);
        var step = 0;

        var xPositive = isPositve();
        var yPositive = isPositve();

        GameObject prevFootstep = null;

        for (int i = 0; i < tracksNumber; i++)
        {

            var basic = (float)distanceZone.close + step;
            var distanceX = Random.Range(basic, basic + 5) * xPositive;
            var distanceZ = Random.Range(basic, basic + 5) * yPositive;

            var distance = new Vector3(distanceX, 0, distanceZ);

            var position = animal.transform.position + distance;

            var indexAnimal = System.Array.FindIndex(AvailableAnimals, row => row.name.Contains(animal.name.Replace("(Clone)", "")));

            var footstep = Instantiate(AvailableFootsteps[indexAnimal], position, Quaternion.identity);

            footstep.name = footstep.name + i;

            instanceInList.addFootstep(footstep);

            //roration of track
            if (!prevFootstep) { prevFootstep = animal.gameObject; };

            var headingAnimal = prevFootstep.transform.position - footstep.transform.position;

            var distanceFromAnimal = headingAnimal.magnitude;
            var directionToPrevFootstep = headingAnimal / distanceFromAnimal;

            footstep.transform.rotation = Quaternion.LookRotation(directionToPrevFootstep);

            prevFootstep = footstep;
            step += 5;
        }


    }

    private int isPositve()
    {
        bool isPositive = Random.Range(0, 10) < 5;
        return (isPositive ? 1 : -1);
    }

    private int isPositveNumber(float number)
    {
        bool isPositive = number >= 0;
        return (isPositive ? 1 : -1);
    }
    private IEnumerator waitAndGo()
    {
        yield return new WaitUntil(() => !_map.CenterMercator.normalized.Equals(new Vector2d(0.00000, 0.00000)));
        // LiveAnimalsManager.Instance.enabledFootprint(animalsInstances[1].Footsteps[1]);
      //  LiveAnimalsManager.Instance.enabledAllFootprints(false);
      //  LiveAnimalsManager.Instance.showAllAnimals(false);
    }

    private IEnumerator waitToMapLoad()
    {
        yield return new WaitUntil(() => !_map.CenterMercator.normalized.Equals(new Vector2d(0.00000, 0.00000)));
        isMapLoad = true;

    }

}
