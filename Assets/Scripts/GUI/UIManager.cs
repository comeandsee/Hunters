using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    [SerializeField] private Camera mapCam;
    [SerializeField] private GameObject ARCam;
    [SerializeField] private GameObject Map;
    [SerializeField] private GameObject distanceInfoBox;
    [SerializeField] private GameObject gameAreaMaxDistance;
    [SerializeField] private InputField maxRangeInputField;
    [SerializeField] private InputField minRangeInputField;
    [SerializeField] private Toggle isLocalGameToogle;

    // public Slider musicVolume;

    private AudioSource audioSource;

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


        //   StartCoroutine(waitToMapLoad()); 
    }

    public void showNewLvlBox(bool setActive=true)
    {
        newLvlBox.gameObject.SetActive(setActive);
    }

    public void showWinnerBox(bool setActive = true)
    {
        winnerBox.gameObject.SetActive(setActive);
    }

    public void showRulesBox(bool setActive = true)
    {
        rulesBox.gameObject.SetActive(setActive);
    }

    public void showSettingsBox(bool setActive = true)
    {
        settingsBox.gameObject.SetActive(setActive);
        maxRangeInputField.text = HuntersConstants.maxRange.ToString();
        minRangeInputField.text = HuntersConstants.minRange.ToString();
        isLocalGame();
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

        var renderers = Map.GetComponentsInChildren<MeshRenderer>();
        foreach ( Renderer r in renderers)
        {
            r.enabled = !r.enabled;
        }
        //tutaj jesli chcesz zeby za kazdym razem inne zwierze sledzilo - najlizsze
    }

    public void StartBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        int lvl = GameManager.Instance.CurrentPlayer.Lvl;
        GameManager.Instance.CurrentPlayer.startFromBeginning( lvl);
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

    public void isGameAreaTooFar()
    {
        double minDistance;
        var player = GameObject.FindWithTag("Player");
        var animal = HuntARManager.Instance.GetHuntedAnimal(out minDistance);

        var distance = GetDistance(player.transform.position, animal.gameObject);


        if (distance >= HuntersConstants.gameAreaMaxDistance)
        {
            gameAreaMaxDistance.gameObject.SetActive(true);
        }
        else
        {
            gameAreaMaxDistance.gameObject.SetActive(false);
        }
    }
    private float GetDistance(Vector3 playerPosition, GameObject gameObject)
    {
        var heading = gameObject.transform.position - playerPosition;
        return heading.magnitude;
    }

    private IEnumerator waitToMapLoad()
    {
        yield return new WaitUntil(() => AnimalFactory.Instance.isMapLoad == true);
        isGameAreaTooFar();

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

    /*  public void SetVolume()
      { 
          AudioSource [] ad  = GetComponents<AudioSource>();
          audioSource.volume = musicVolume.value;
      }*/
}
