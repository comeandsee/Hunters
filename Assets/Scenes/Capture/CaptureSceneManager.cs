using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureSceneManager : HuntersSceneManager
{
    public override void animalTapped(GameObject animal)
    {
        print("1 aniaml gartki ");
    }

    public override void playerTapped(GameObject player)
    {
        print("2 palyer gartki ");
    }
}
