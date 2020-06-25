using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSceneManager : HuntersSceneManager
{
    private GameObject animal;
    private AsyncOperation loadScene;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void playerTapped(GameObject player)
    {

    }

    public override void animalTapped(GameObject animalObject)
    {
        Animal animal = animalObject.GetComponent<Animal>();
        AnimalFactory.Instance.AnimalWasSelected(animal);

        List<GameObject> objects = new List<GameObject>();
        //objects.Add(animalObject);
       // DontDestroyOnLoad(animal);
        SceneTransitionManager.Instance.
            GoToScene(HuntersConstants.SCENE_CAPTURE, objects);
   }


}

