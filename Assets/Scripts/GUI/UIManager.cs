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

    public void updateLevel(int lvl)
    {
        lvlText.text = lvl.ToString();
    }

    public void updateXP(int currentXP, int requiredXP)
    {
        xpText.text = currentXP.ToString() + " / " + requiredXP.ToString();
    }

    public void toggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

}
