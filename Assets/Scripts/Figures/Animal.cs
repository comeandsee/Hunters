using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Mapbox.Unity.Location;
using System;
using UnityEngine.UI;

public class Animal : MonoBehaviour
{


    [SerializeField] private float spawnRate = 0.10f;
    [SerializeField] private float catchRate = 0.10f;
    [SerializeField] private int attack = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private AudioClip animalSound;
    [SerializeField] private int hp = 80;


    private AudioSource audioSource;
    private float maxDistance = 15.0f;
   // private Vector3 position;

    public Transform debrisObj;
    public string debrisDelay = "n";
    List<Transform> generatedObjects = new List<Transform>();

    private Vector3 positionStart = new Vector3();

    private void Awake()
    {

        AudioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(AudioSource);
        Assert.IsNotNull(AnimalSound);

    }
    private void Start()
    {
        DontDestroyOnLoad(this);

    /*    if (null == _locationProvider)
        {
            _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }*/
    }

    public float SpawnRate
    {
        get { return spawnRate; }
        set {  spawnRate=value; }
    }

    public float CatchRate
    {
        get { return catchRate; }
        set { catchRate = value; }
    }

    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    public AudioClip AnimalSound { get => animalSound; set => animalSound = value; }

    // public Vector3 Position { get => position; set => position = value; }

    void Update()
    {
       /* if (Hp < 1)
        {
            AnimalFactory.Instance.gatherAnimal(this);
            Destroy(gameObject);
           
        }*/
    }

    private AbstractLocationProvider _locationProvider = null;
    private void OnMouseDown()
    {
       // Location currLoc = _locationProvider.CurrentLocation;
     //   var a = currLoc.LatitudeLongitude;

        var animalPosition = this.gameObject.transform.position;
        var userPosition = GameManager.Instance.CurrentPlayer.transform.position;
        var xDistance = Math.Abs(userPosition.x - animalPosition.x);
        var zDistance = Math.Abs(userPosition.z - animalPosition.z);

        if (xDistance <= maxDistance && zDistance <= maxDistance)
        {

            HuntersSceneManager[] managers = FindObjectsOfType<HuntersSceneManager>();
            AudioSource.PlayOneShot(AnimalSound);
            foreach (HuntersSceneManager huntersSceneManager in managers)
            {
                if (huntersSceneManager.gameObject.activeSelf)
                {

                    positionStart = this.gameObject.transform.position;
                    this.gameObject.transform.position = new Vector3(0.52f, -3.15f, 9.26f);

                    huntersSceneManager.animalTapped(this.gameObject);
                }
            }
        }
        else{
      
            var uI = FindObjectOfType<UIManager>(); ;
            uI.showPositionBox();
            StartCoroutine(WaitAndNotShowTxt(1.0f));
        }
    }
  


private IEnumerator WaitAndNotShowTxt(float waitTime)
{
    yield return new WaitForSeconds(waitTime);
    var uI = FindObjectOfType<UIManager>(); ;
    uI.hidePositionBox();

}

public List<Transform> getGeneratedObjects()
    {
        return generatedObjects;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(HuntersConstants.TAG_Harvester))
        {
            hp -= 1;

            if (hp == 0)
            {
                HuntersSceneManager[] managers = FindObjectsOfType<HuntersSceneManager>();
                foreach (HuntersSceneManager huntersSceneManager in managers)
                {
                    if (huntersSceneManager.gameObject.activeSelf)
                    {
                        {
                            huntersSceneManager.animalCollision(this.gameObject, other);
                            AnimalFactory.Instance.gatherAnimal(this);
                            Destroy(gameObject);
                        }
                    }
                }

                // harvester destroy
                Harvest harvest = FindObjectOfType<Harvest>();
                harvest.gameObject.Destroy();
            }


            if (debrisDelay == "n")
            {
                var objInst = Instantiate(debrisObj, transform.position, debrisObj.rotation);
                generatedObjects.Add(objInst);

                debrisDelay = "y";
                StartCoroutine(resetDelay());
            }


            
        }
    }


    IEnumerator resetDelay()
    {
        yield return new WaitForSeconds(.35f);
        debrisDelay = "n";

    }

    IEnumerator deletDebris(Transform obj)
    {
        yield return new WaitForSeconds(2.0f);
        Destroy((obj as Transform).gameObject);
    }

    public void hideObject()
    {
        this.gameObject.SetActive(false);
    }

    public void showObject()
    {
        this.gameObject.SetActive(true);
    }

    public void loadFromAnimalData(AnimalData data)
    {

        SpawnRate = data.SpawnRate;
        CatchRate = data.CatchRate;
        Attack = data.Attack;
        Defense = data.Defense;
        Hp = data.Hp;
        //AnimalSound = Resources.Load() as AudioClip;
        AnimalSound = Resources.Load<AudioClip>("Audio/"+ data.AnimalSound);

    }

    public void changePositionToDefault()
    {
        this.gameObject.transform.position = positionStart;
    }
}