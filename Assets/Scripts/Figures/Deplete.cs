using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Deplete : MonoBehaviour
{
    // Start is called before the first frame update
    public int resourceHP = 60;
    public Transform debrisObj;
    public string debrisDelay = "n";
    List<Transform> generatedObjects = new List<Transform>();



    public List<Transform> getGeneratedObjects()
    {
        return generatedObjects;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(resourceHP < 1)
        {
            Destroy(gameObject);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag(HuntersConstants.TAG_Harvester))
        {
            resourceHP -= 1;
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

    IEnumerator deletDebris(Transform obj )
    {
        yield return new WaitForSeconds(2.0f);
        Destroy((obj as Transform).gameObject);
    }
}
