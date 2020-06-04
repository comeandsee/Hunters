using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    private  int gemUnits = 0;
    private  int wolfUnits = 0;
    public KeyCode fireLaser;

    public  string laserInUse = "n";
    private  int overheat = 0;
    public  TimeSpan time= new TimeSpan();
    System.Diagnostics.Stopwatch stopWatch;
    List<Transform> generatedObjects = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
       
      

    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetKeyDown(fireLaser)) && (laserInUse == "n"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6);
            laserInUse = "y";
        }
        if (laserInUse == "y")
        {
            overheat += 1;
        }
        if ((laserInUse == "locked") && (overheat > 0))
        {
            overheat -= 1;

        }
   
    
        if (overheat > 300)
        {
            laserInUse = "locked";
              GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -6);
            float waitTime = 3.0f;
            if (gemUnits > 0)
            {
                waitTime = 2;
            }
            StartCoroutine(StopLaser(waitTime));
        }
        Debug.Log("overhead: " + overheat);
   
    }

    private void OnTriggerStay(Collider other)
    {
        gemUnits += 1;
      /*  if (other.name == "gem")
        {
            gemUnits += 1;
        }
        if (other.name == "Wolf")
        {
            wolfUnits += 1;
        }
        if (other.name == "zoomObj")
        {
            generatedObjects=other.gameObject.GetComponent<Deplete>().getGeneratedObjects();
            gemUnits += 1;
        }*/
        Debug.Log("gem " + gemUnits + " wolf " + wolfUnits);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        Deplete deplte = FindObjectOfType<Deplete>();
        generatedObjects = deplte.getGeneratedObjects();

    }

    IEnumerator StopLaser(float timeToWait = 4f)
    {
  
        yield return new WaitForSeconds(timeToWait);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        laserInUse = "n";
       foreach(Transform obj in generatedObjects)
        {
            Destroy(obj.gameObject);
        }
   //     Destroy(gameObject);

    }

}
