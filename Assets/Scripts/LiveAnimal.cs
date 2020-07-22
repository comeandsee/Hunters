using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveAnimal
{

    private Animal animal;
    private List<GameObject> footsteps = new List<GameObject>();

    public LiveAnimal(Animal animal)
    {
        this.animal = animal;
        this.footsteps = new List<GameObject>();
    }

    public Animal Animal { get => animal;  }


    public void addFootstep(GameObject gameObject)
    {
        this.Footsteps.Add(gameObject);
    }

    public void destroyAllFootsteps()
    {
        foreach (var footstep in this.Footsteps)
        {
            footstep.Destroy();
        }
    }


    public List<GameObject> Footsteps { get => footsteps; }


 
}
