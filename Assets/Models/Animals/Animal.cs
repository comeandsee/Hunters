using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Animal  : MonoBehaviour
{


    [SerializeField] private float spawnRate = 0.10f;
    [SerializeField] private float catchRate = 0.10f;
    [SerializeField] private int attack = 0;
    [SerializeField] private int defense = 0;
    [SerializeField] private int hp = 10;
    //[SerializeField] private AudioClip crySound;

    //private AudioSource audioSource;

    private void Awake()
    {
      //  audioSource = GetComponent<AudioSource>();
      //  Assert.IsNotNull(audioSource);
      //  Assert.IsNotNull(crySound);
    }

    private void Start()
    {
        //		DontDestroyOnLoad(this);
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
        /*PocketDroidsSceneManager[] managers = FindObjectsOfType<PocketDroidsSceneManager>();
        audioSource.PlayOneShot(crySound);
        foreach (PocketDroidsSceneManager pocketDroidsSceneManager in managers)
        {
            if (pocketDroidsSceneManager.gameObject.activeSelf)
            {
                pocketDroidsSceneManager.droidTapped(this.gameObject);
            }
        }*/
    }
    /* 
   private void OnCollisionEnter(Collision other)
   {
       PocketDroidsSceneManager[] managers = FindObjectsOfType<PocketDroidsSceneManager>();
       foreach (PocketDroidsSceneManager pocketDroidsSceneManager in managers)
       {
           if (pocketDroidsSceneManager.gameObject.activeSelf)
           {
               pocketDroidsSceneManager.droidCollision(this.gameObject, other);
           }
       }
   }
   */
}
