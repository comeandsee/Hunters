using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HuntersSceneManager : MonoBehaviour
{
    public abstract void playerTapped(GameObject player);
    public abstract void animalTapped(GameObject animal);

    public virtual void animalCollision(GameObject animal, Collider other) { }
}
