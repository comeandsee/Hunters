using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Animal : MonoBehaviour
{


    [SerializeField] private float spawnRate = 0.10f;
    [SerializeField] private float catchRate = 0.10f;
    [SerializeField] private int attack = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private AudioClip animalSound;
    [SerializeField] private int hp = 80;

    private AudioSource audioSource;


    public Transform debrisObj;
    public string debrisDelay = "n";
    List<Transform> generatedObjects = new List<Transform>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
        Assert.IsNotNull(animalSound);
    }

    public float SpawnRate
    {
        get { return spawnRate; }
    }

    public float CatchRate
    {
        get { return catchRate; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int Hp
    {
        get { return hp; }
    }

    void Update()
    {
        if (Hp < 1)
        {
            Destroy(gameObject);

        }
    }


    private void OnMouseDown()
    {
        HuntersSceneManager[] managers = FindObjectsOfType<HuntersSceneManager>();
        audioSource.PlayOneShot(animalSound);
        foreach (HuntersSceneManager huntersSceneManager in managers)
        {
            if (huntersSceneManager.gameObject.activeSelf)
            {
                huntersSceneManager.animalTapped(this.gameObject);
            }
        }
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
            if (debrisDelay == "n")
            {
                var objInst = Instantiate(debrisObj, transform.position, debrisObj.rotation);
                generatedObjects.Add(objInst);

                debrisDelay = "y";
                StartCoroutine(resetDelay());
            }


            if(hp == 0)
            {
                HuntersSceneManager[] managers = FindObjectsOfType<HuntersSceneManager>();
                foreach (HuntersSceneManager huntersSceneManager in managers)
                {
                    if (huntersSceneManager.gameObject.activeSelf)
                    {
                        {
                            huntersSceneManager.animalCollision(this.gameObject, other);
                        }
                    }
                }

                // harvester destroy
                Harvest harvest = FindObjectOfType<Harvest>();
                harvest.gameObject.Destroy();
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
}