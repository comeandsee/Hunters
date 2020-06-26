using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        var arrayOfChildrenOfAnimal = animal.transform
            .Cast<Transform>()
            .Where(c => c.gameObject.tag == "Animal").Select(c => c.gameObject)
            .ToArray();
 

        Animal[] allAnimals = FindObjectsOfType<Animal>();
        foreach(Animal a in allAnimals)
        {
            a.hideObject();
        }

        foreach (GameObject childObj in arrayOfChildrenOfAnimal)
        {
            var child = childObj.GetComponent<Animal>();
            child.showObject();

        }

        animal.showObject();
        
        SceneTransitionManager.Instance.
            GoToScene(HuntersConstants.SCENE_CAPTURE, new List<GameObject>());
   }


}

