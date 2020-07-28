using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HuntersConstants
{
    public static string SCENE_WORLD = "World";
    public static string SCENE_CAPTURE = "Capture";

    public static string TAG_ANIMAL = "Animal";
    public static string TAG_Harvester = "Harvester";

    public static Vector3 objectPositionInCaptureScene = new Vector3(-2.03f, -7.42f, 20.1f);
    public static Vector3 objectRotationInCaptureScene = new Vector3(0f, -30f, 0f);

    public static int maxLvl = 5;
    public static float maxDistance = 100.0f;

    public static int startingAnimals = 6;
    public static float minRange = 5.0f;
    public static float maxRange = 100.0f;

    public static bool isLocalGame = false;
    public static bool isDebugMode = false;
    public static bool isGdansk = false;

    public static float gameAreaMaxDistance = 100.0f;



    public enum distanceZone
    {
        close = 5, 
        middle = 10,
        away = 15,
        tooFar
    }

    public static (Vector3, Vector3) CamOnNorth() => (new Vector3(0, 44, -37), new Vector3(44, 0, 0));
    public static (Vector3, Vector3) CamOnWest() => (new Vector3(-37, 44, 0), new Vector3(44, 90, 0));
    public static (Vector3, Vector3) CamOnSouth() => (new Vector3(0, 44, 37), new Vector3(44, 180, 0));
    public static (Vector3, Vector3) CamOnEast() => (new Vector3(37, 44, 0), new Vector3(44, -90, 0));

}