﻿using System.Collections;
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

    public static int startingAnimals = 5;
    public static float minRange = 2.0f;
    public static float maxRange = 20.0f;
}