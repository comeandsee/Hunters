using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Mapbox.Unity.Location;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Animal : MonoBehaviour
{

    [SerializeField] private AudioClip animalSound;
    [SerializeField] private int hp =0;
    [SerializeField] private int points = 0;
    [SerializeField] private int lvl = 1;

    private AudioSource audioSource;

    public Transform debrisObj;
    public string debrisDelay = "n";
    List<Transform> generatedObjects = new List<Transform>();

    private Vector3 positionStart = new Vector3();


    private bool isGathering = false;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(AudioSource);
        Assert.IsNotNull(AnimalSound);

    }
    private void Start()
    {
        DontDestroyOnLoad(this);
    }



    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    public AudioClip AnimalSound { get => animalSound; set => animalSound = value; }
    public int Points { get => points; set => points = value; }
    public int Lvl { get => lvl; set => lvl = value; }

    

    private void OnMouseDown()
    {

        StartCoroutine(clickedAnimalSound());

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == HuntersConstants.SCENE_WORLD)
        {
           
            var animalPosition = this.gameObject.transform.position;

            var player = GameObject.FindWithTag("Player");
            var userPosition = player.transform.position;

            var xDistance = Math.Abs(userPosition.x - animalPosition.x);
            var zDistance = Math.Abs(userPosition.z - animalPosition.z);

            if (xDistance <= HuntersConstants.maxDistance && zDistance <= HuntersConstants.maxDistance)
            {

                HuntersSceneManager[] managers = FindObjectsOfType<HuntersSceneManager>();
                foreach (HuntersSceneManager huntersSceneManager in managers)
                {
                    if (huntersSceneManager.gameObject.activeSelf)
                    {
                        if (!HuntersConstants.isAreaGame)
                        {
                            positionStart = this.gameObject.transform.position;
                            this.gameObject.transform.position = HuntersConstants.objectPositionInCaptureScene;
                            this.gameObject.transform.eulerAngles = HuntersConstants.objectRotationInCaptureScene;
                        }
                        huntersSceneManager.animalTapped(this.gameObject);
                    }
                }
            }
            else
            {

                var uI = FindObjectOfType<UIManager>();
                uI.showPositionBox();
                StartCoroutine(WaitAndNotShowTxt(1.0f));
            }
            
        }
        
    }
  


private IEnumerator WaitAndNotShowTxt(float waitTime)
{
    yield return new WaitForSeconds(waitTime);
    var uI = FindObjectOfType<UIManager>(); ;
    uI.showPositionBox(false);

}

public List<Transform> getGeneratedObjects()
    {
        return generatedObjects;
    }

  /*  private void OnTriggerStay(Collider other)
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
                            huntersSceneManager.animalCollision();
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

    */


    public void StartGatherInWordScene()
    {
        hp -= 1;
        if (hp == 0)
        {
            AnimalFactory.Instance.gatherAnimal(this);
            Destroy(gameObject);

        }
        CreateParticles();
       
    }
    public void GatherInCaptureScene()
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
                        huntersSceneManager.animalCollision();
                        AnimalFactory.Instance.gatherAnimal(this);
                        Destroy(gameObject);
                    }
                }
            }

            // harvester destroy
            Harvest harvest = FindObjectOfType<Harvest>();
            harvest.gameObject.Destroy();
        }

        CreateParticles();

    }

    private void CreateParticles()
    {
        if (debrisDelay == "n")
        {
            var objInst = Instantiate(debrisObj, transform.position, debrisObj.rotation);
            generatedObjects.Add(objInst);

            debrisDelay = "y";
            StartCoroutine(resetDelay());
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

        Hp = data.Hp;
        Points = data.Points;
        //AnimalSound = Resources.Load() as AudioClip;
        AnimalSound = Resources.Load<AudioClip>("Audio/"+ data.AnimalSound);

    }

    public void changePositionToDefault()
    {
        this.gameObject.transform.position = positionStart;
    }

    public void deleteMe()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator clickedAnimalSound()
    {
        AudioSource.PlayOneShot(AnimalSound);
        yield return new WaitForSeconds(2f);
    }
}