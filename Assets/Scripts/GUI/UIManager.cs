﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static HuntersConstants;
using static Location;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text xpText;
    [SerializeField] private Text lvlText;
    [SerializeField] private Text distanceInfoText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject postionBox;
    [SerializeField] private GameObject huntBox;
    [SerializeField] private GameObject newLvlBox;
    [SerializeField] private GameObject winnerBox;
    [SerializeField] private GameObject rulesBox;
    [SerializeField] private GameObject settingsBox;
    [SerializeField] private AudioClip menuBtnSound;
    [SerializeField] private AudioClip camerShootBtnSound;
    [SerializeField] private Camera mapCam;
    [SerializeField] private GameObject ARCam;
    [SerializeField] private GameObject Map;
    [SerializeField] private GameObject distanceInfoBox;
    [SerializeField] private GameObject gameAreaMaxDistance;
    [SerializeField] private InputField maxRangeInputField;
    [SerializeField] private InputField minRangeInputField;
    [SerializeField] private Toggle isLocalGameToogle;
    [SerializeField] private Toggle isDebugModeToogle;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject camRotationButton;
    [SerializeField] private GameObject huntedAnimalHPBox;
    [SerializeField] private GameObject aboutBox;
    [SerializeField] private GameObject debugBox;
    [SerializeField] private GameObject screenshootButton;

    [SerializeField] private GameObject SuccessBox;
    [SerializeField] private Text animalNameTxt;
    [SerializeField] private Text animalPointsTxt;
    [SerializeField] private Text huntedAnimalHPTxt;
    [SerializeField] private Text debugtxt;

    // public Slider musicVolume;

    private AudioSource audioSource;
    private int camPositionNumber = 1;
    private int screenNameNumber = 1;
   
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(audioSource);
        Assert.IsNotNull(xpText);
        Assert.IsNotNull(lvlText);
        Assert.IsNotNull(menu);
        Assert.IsNotNull(menuBtnSound);
        Assert.IsNotNull(postionBox);
        Assert.IsNotNull(winnerBox);
        Assert.IsNotNull(mapCam);
        Assert.IsNotNull(ARCam);
        Assert.IsNotNull(Map);
        Assert.IsNotNull(huntedAnimalHPBox);


        //   StartCoroutine(waitToMapLoad()); 
    }

    public void showNewLvlBox(bool setActive = true)
    {
        newLvlBox.gameObject.SetActive(setActive);
    }

    public void showHuntedAnimalHPBox(bool setActive = true)
    {
        huntedAnimalHPBox.gameObject.SetActive(setActive);
    }

    public void showWinnerBox(bool setActive = true)
    {
        winnerBox.gameObject.SetActive(setActive);
    }

    public void showRulesBox(bool setActive = true)
    {
        rulesBox.gameObject.SetActive(setActive);
    }

    public void showCamRotationButton(bool setActive = true)
    {
        camRotationButton.gameObject.SetActive(setActive);
    }

    public void showSettingsBox(bool setActive = true)
    {
        settingsBox.gameObject.SetActive(setActive);
        maxRangeInputField.text = HuntersConstants.maxRange.ToString();
        minRangeInputField.text = HuntersConstants.minRange.ToString();
        isLocalGame();
        isDebugMode();
    }

    public void showPositionBox(bool setActive = true)
    {
        postionBox.gameObject.SetActive(setActive);
    }

    public void showHuntBox(bool setActive = true)
    {
        huntBox.gameObject.SetActive(setActive);
    }

    private void updateDistanceInf(string info)
    {
        distanceInfoText.text = info;
    }
    public void updateLevel()
    {
        lvlText.text = "Lvl: " + GameManager.Instance.CurrentPlayer.Lvl.ToString();
    }

    public void updateXP()
    {
        xpText.text = GameManager.Instance.CurrentPlayer.Xp + " / " + GameManager.Instance.CurrentPlayer.RequiredXp;
    }

    private void toggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    private void Update()
    {
        updateLevel();
        updateXP();
        UpdateDistanceToNearestAnimal();
        updateHuntedAnimalHPText();
    }

    private void updateHuntedAnimalHPText()
    {
        if (!huntedAnimalHPTxt.IsActive()) return;
        huntedAnimalHPTxt.text = AnimalFactory.Instance.SelectedAnimal.Hp.ToString();
    }

    public void MenuBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        toggleMenu();
    }

    public void ChangeCamBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        changeCam();
    }

    public void changeCam()
    {

        mapCam.enabled = !mapCam.enabled;
        ARCam.SetActive(!ARCam.activeSelf);
        distanceInfoBox.gameObject.SetActive(!distanceInfoBox.activeSelf);

        
        if (mapCam.enabled) playerBody.SetActive(true);
        else if (HuntersConstants.isDebugMode) playerBody.SetActive(true);
        else playerBody.SetActive(false);


        var renderers = Map.GetComponentsInChildren<MeshRenderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = !r.enabled;
        }

        showCamRotationButton(!camRotationButton.activeSelf);
    }

    public void StartBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        int lvl = GameManager.Instance.CurrentPlayer.Lvl;
        GameManager.Instance.CurrentPlayer.startFromBeginning(lvl);
        toggleMenu();

    }

    public void RulesBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        showRulesBox();
    }
    public void SettingsBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        showSettingsBox();
    }

    public void CamRotationBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);

        camPositionNumber += 1;
        if (camPositionNumber == 5) camPositionNumber = 1;

        var player = GameObject.FindWithTag("Player");
        (Vector3, Vector3) camSetting;

        switch (camPositionNumber)
        {
            case 1:
                camSetting = HuntersConstants.CamOnNorth();
                break;
            case 2:
                camSetting = HuntersConstants.CamOnEast();
                break;
            case 3:
                camSetting = HuntersConstants.CamOnSouth();
                break;
            case 4:
                camSetting = HuntersConstants.CamOnWest();
                break;
            default:
                camSetting = HuntersConstants.CamOnNorth();
                break;
        }


        mapCam.transform.position = camSetting.Item1 + player.transform.position;
        mapCam.transform.rotation = Quaternion.Euler(camSetting.Item2);



    }



    public IEnumerator WaitAndHideNewLvlBox(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        showNewLvlBox(false);
    }


    public void StartAgainBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        showWinnerBox(false);
        GameManager.Instance.CurrentPlayer.startFromBeginning();
    }

    public void ExitRulesBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        showRulesBox(false);
        showSettingsBox(false);
    }


    public void updateDistanceStatusUI(distanceZone distanceState)
    {
        switch (distanceState)
        {
            case distanceZone.close:
                updateDistanceInf("look around");
                break;
            case distanceZone.middle:
                updateDistanceInf("you are near");
                break;
            case distanceZone.away:
                updateDistanceInf("come closer");
                break;
            case distanceZone.tooFar:
                updateDistanceInf("you are far away");
                break;
            default:
                break;
        }
    }


    private float GetDistance(Vector3 playerPosition, GameObject gameObject)
    {
        var heading = gameObject.transform.position - playerPosition;
        return heading.magnitude;
    }



    public void isLocalGameChanged(Toggle change)
    {
        HuntersConstants.isLocalGame = change.isOn;
        StartNewGame();
    }

    private static void StartNewGame()
    {
        int lvl = GameManager.Instance.CurrentPlayer.Lvl;
        GameManager.Instance.CurrentPlayer.startFromBeginning(lvl);
    }

    public void isLDebugModeChanged(Toggle change)
    {
        HuntersConstants.isDebugMode = change.isOn;
        if (change.isOn)
        {
            showDebugTxt();
            playerBody.gameObject.SetActive(true);
        }
        else
        {
            showDebugTxt(false);
            if(ARCam.activeSelf) playerBody.gameObject.SetActive(false);
        }

    }

    public void minRange(InputField input)
    {
        HuntersConstants.minRange = float.Parse(input.text, CultureInfo.InvariantCulture.NumberFormat);
        StartNewGame();
    }

    public void maxRange(InputField input)
    {
        HuntersConstants.maxRange = float.Parse(input.text, CultureInfo.InvariantCulture.NumberFormat);
        StartNewGame();
    }

    private void isLocalGame()
    {
        isLocalGameToogle.isOn = HuntersConstants.isLocalGame;
    }

    private void isDebugMode()
    {
        isDebugModeToogle.isOn = HuntersConstants.isDebugMode;
       
    }


    private void UpdateDistanceToNearestAnimal()
    {
        double minDistance;
        var player = GameObject.FindWithTag("Player");
        bool isGameAreaTooFar = true;


        var animal = HuntARManager.Instance.GetHuntedAnimal(out minDistance);

        if (animal == null)
        {
            isGameAreaTooFar = true;
        }
        else
        {
            var distanceState = HuntARManager.Instance.updateDistanceStatus(minDistance);

            updateDistanceStatusUI(distanceState);

            if (minDistance < HuntersConstants.maxDistance)
            {
                isGameAreaTooFar = false;
            }
        }
        gameAreaMaxDistance.gameObject.SetActive(isGameAreaTooFar);

    }


    public void ShowSuccessBox()
    {
        SuccessBox.gameObject.SetActive(true);
        animalNameTxt.text = AnimalFactory.Instance.NameSelectedAnimal.Replace("(Clone)", "");
        animalPointsTxt.text = "+ " + AnimalFactory.Instance.PointsSelectedAnimal.ToString();

        StartCoroutine(waitAndCloseSuccessBox());
    }

    private IEnumerator waitAndCloseSuccessBox()
    {
        yield return new WaitForSeconds(2.0f);
        SuccessBox.gameObject.SetActive(false);

    }

    public void exitGame()
    {
        audioSource.PlayOneShot(menuBtnSound);
        Application.Quit();
    }

    public void aboutBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        aboutBox.gameObject.SetActive(!aboutBox.activeSelf);
    }

    public void exitAboutBox()
    {
        aboutBox.gameObject.SetActive(false);
    }

    public void getScreenshot()
    {
        audioSource.PlayOneShot(camerShootBtnSound);

#if UNITY_EDITOR
         StartCoroutine(TakeScreenshotOnEditor());
#else
        StartCoroutine(TakeScreenshotOnAndroid());
#endif

    }


    public IEnumerator TakeScreenshotOnAndroid()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture(ScreenCapture.StereoScreenCaptureMode.BothEyes);

        string name = PhotoShoot.UniqueNameScreenshot();
        var cos = PhotoShoot.SaveImageToGallery(texture, name, "Hunters screenshot");
    }


    public IEnumerator TakeScreenshotOnEditor()
    {

        var myFileName = PhotoShoot.UniqueNameScreenshot();
        yield return new WaitForEndOfFrame();

        string dirPath = Path.Combine(Application.persistentDataPath, "HuntersScreenshots");
        string filePath = Path.Combine(dirPath, myFileName + ".png");
        
        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(dirPath);
                yield return new WaitForSeconds(4);
            }

        ScreenCapture.CaptureScreenshot(filePath);
    }

    public void showDebugTxt(bool setActive = true)
    {
        debugBox.gameObject.SetActive(setActive);
      //  debugtxt.gameObject.SetActive(setActive);
    }


    public void addDebugTxt(string text)
    {
       
        if (HuntersConstants.isDebugMode)
        {
            debugtxt.text += "\n" + text ;
        }
    }

    public void clearDebugTxt()
    {
        if (HuntersConstants.isDebugMode)
        {
            debugtxt.text = " ";
        }
    }
    public void showScreenshootButton(bool setActive = true)
    {
        screenshootButton.gameObject.SetActive(setActive);
    }

    /*  public void SetVolume()
      { 
          AudioSource [] ad  = GetComponents<AudioSource>();
          audioSource.volume = musicVolume.value;
      }*/
}


