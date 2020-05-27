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

        var a = laserInUse;
        if ( (laserInUse == "n") )
        {
            stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6);
            laserInUse = "y";
       //     StartCoroutine(WaitASec());



        }
      /*  if ( (Input.GetKeyUp(fireLaser)) && (laserInUse == "y"))
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts;

            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -6);
            StartCoroutine(StopLaser());
            laserInUse = "n";
        }*/

        if (laserInUse == "y")
        {
            overheat += 1;
        }
        if ( (laserInUse=="n") && (overheat > 0))
        {
            overheat = 0;
           
        }
    //    if ((laserInUse == "locked") && (overheat > 0))
        {
      //      overheat -= 1;

        }


        if (overheat > 300 )
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            time = ts;

            laserInUse = "locked"; //locked
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -6);
            StartCoroutine(StopLaser());
        }
        
        Debug.Log("overhead: " + overheat);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "gem")
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
        }
        Debug.Log("gem " + gemUnits + " wolf " + wolfUnits);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
 

    }
    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(1.7f);

    }

    IEnumerator StartLaser()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6);
        yield return new WaitForSeconds(2);
       // laserInUse = "n";
    }
    IEnumerator StopLaser()
    {
        var timeToWait = 1.7f;
        /* var timeToWait = (float)time.TotalSeconds;
         if (timeToWait == 0)
         {
             timeToWait = 1.7f;
         }*/
        yield return new WaitForSeconds(timeToWait);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        laserInUse = "n";
        var a =generatedObjects;
        foreach(Transform obj in generatedObjects)
        {
            Destroy(obj.gameObject);
        }
        Destroy(gameObject);

       

    }

}
