using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text xpText;
    [SerializeField] private Text lvlText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject postionBox;
    [SerializeField] private GameObject newLvlBox;
    [SerializeField] private AudioClip menuBtnSound;

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

    }

    public void showNewLvlBox(bool setActive=true)
    {
        newLvlBox.gameObject.SetActive(setActive);
    }

    
    public void showPositionBox(bool setActive = true)
    {
        postionBox.gameObject.SetActive(setActive);
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

    public void StartBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        //todo
    }

    public void RankingBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        //todo
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
}
