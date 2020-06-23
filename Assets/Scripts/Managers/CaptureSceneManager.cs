using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureSceneManager : HuntersSceneManager
{
    private CaptureSceneStatus status = CaptureSceneStatus.InProgress;


    public CaptureSceneStatus Status
    {
        get { return status; }
    }
    public override void animalTapped(GameObject animal)
    {
        print("1 aniaml gartki ");
    }

    public override void playerTapped(GameObject player)
    {
        print("2 palyer gartki ");
    }

    public override void animalCollision(GameObject animal, Collider other)
    {
        status = CaptureSceneStatus.Successful;
            
    }
}
