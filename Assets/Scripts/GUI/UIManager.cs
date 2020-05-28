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

    private void Awake()
    {
        Assert.IsNotNull(xpText);
        Assert.IsNotNull(lvlText);
        Assert.IsNotNull(menu);

    }

    public void updateLevel()
    {
        lvlText.text = GameManager.Instance.CurrentPlayer.Lvl.ToString();
    }

    public void updateXP()
    {
        xpText.text = GameManager.Instance.CurrentPlayer.Xp + " / " + GameManager.Instance.CurrentPlayer.RequiredXp;
    }

    public void toggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    private void Update()
    {
        updateLevel();
        updateXP();
    }

}
