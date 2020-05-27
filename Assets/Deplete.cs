using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplete : MonoBehaviour
{
    // Start is called before the first frame update
    public int resourceHP = 80;
    public Transform debrisObj;
    public string debrisDelay = "n";
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
        resourceHP -= 1;
        if(debrisDelay == "n")
        {
            Instantiate(debrisObj, transform.position, debrisObj.rotation);
            debrisDelay = "y";
            StartCoroutine(resetDelay());
        }
    }


    IEnumerator resetDelay()
    {
        yield return new WaitForSeconds(.35f);
        debrisDelay = "n";

    }
}
