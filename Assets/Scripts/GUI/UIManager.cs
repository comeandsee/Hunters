using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

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
    [SerializeField] private AudioClip menuBtnSound;
    [SerializeField] private Camera mapCam;
    [SerializeField] private GameObject ARCam;
    [SerializeField] private GameObject Map;
    [SerializeField] private GameObject distanceInfoBox;
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


    public void showPositionBox(bool setActive = true)
    {
        postionBox.gameObject.SetActive(setActive);
    }

    public void showHuntBox(bool setActive = true)
    {
        huntBox.gameObject.SetActive(setActive);
    }

    public void updateDistanceInf(string info)
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
        Map.SetActive(!Map.activeSelf);
        distanceInfoBox.gameObject.SetActive(!distanceInfoBox.activeSelf);
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
        //todo
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
    }




  /*  public void SetVolume()
    { 
        AudioSource [] ad  = GetComponents<AudioSource>();
        audioSource.volume = musicVolume.value;
    }*/
}
