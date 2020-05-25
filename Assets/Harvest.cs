using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvest : MonoBehaviour
{
    public static int gemUnits = 0;
    public static int wolfUnits = 0;
    public KeyCode fireLaser;

    public  string laserInUse = "n";
    public static int overheat = 0;
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
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 6);
            laserInUse = "y";
           // Thread.Sleep(2);

        }
        if ( (Input.GetKeyUp(fireLaser)) && (laserInUse == "y"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -6);
            StartCoroutine(StopLaser());
            laserInUse = "n";
        }

        if (laserInUse == "y")
        {
            overheat += 1;
        }
        if ( (laserInUse=="n") && (overheat > 0))
        {
            overheat -= 1;
        }

        if(overheat > 200 )//&& laserInUse!=-1)
        {
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


    IEnumerator StopLaser()
    {
        yield return new WaitForSeconds(2.5f);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        laserInUse = "n";
    }

}
