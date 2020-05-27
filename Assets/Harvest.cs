using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    public static int gemUnits = 0;
    public static int wolfUnits = 0;
    public KeyCode fireLaser;

    public  string laserInUse = "n";
    public static int overheat = 0;
    public  TimeSpan time= new TimeSpan();
    System.Diagnostics.Stopwatch stopWatch;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        var a = laserInUse;
        if ((Input.GetKeyDown(fireLaser)) && (laserInUse == "n") )
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


        if (overheat > 400 )
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
    }

}
