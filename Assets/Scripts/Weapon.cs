using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public GameObject harvester;

    public Text weaponButtonText;
    public Button weaponsButton;

    static bool clickedCreateWeapon = false;
    private GameObject instaHarvester;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (!instaHarvester && clickedCreateWeapon==true)
        {
            weaponButtonText.text = "Zbieraj";
            clickedCreateWeapon = false;
            weaponsButton.interactable = true;
        }
        if (clickedCreateWeapon)
        {
            weaponButtonText.text = "-";
            weaponsButton.interactable = false;
        }

    }

    public void createWeapon()
    {
        instaHarvester = Instantiate(harvester, new Vector3(0,-1,-7), transform.rotation); // Quaternion.identity

        clickedCreateWeapon = true;
    }
}
