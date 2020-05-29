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
    [SerializeField] private int hp = 10;
    [SerializeField] private AudioClip animalSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
        Assert.IsNotNull(animalSound);
    }

    private void Start()
    {
   		DontDestroyOnLoad(this);
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

  private void OnMouseDown()
    {
        AnimalSceneManager[] managers = FindObjectsOfType<AnimalSceneManager>();
        audioSource.PlayOneShot(animalSound);
        foreach (AnimalSceneManager animalsSceneManager in managers)
        {
            if (animalsSceneManager.gameObject.activeSelf)
            {
                animalsSceneManager.animalTapped(this.gameObject);
            }
        }
    }
    /* 
   private void OnCollisionEnter(Collision other)
   {
       PocketdroidsSceneManager[] managers = FindObjectsOfType<PocketdroidsSceneManager>();
       foreach (PocketdroidsSceneManager pocketdroidsSceneManager in managers)
       {
           if (pocketdroidsSceneManager.gameObject.activeSelf)
           {
               pocketdroidsSceneManager.droidCollision(this.gameObject, other);
           }
       }
   }
   */
}
