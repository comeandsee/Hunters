using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class XPBonus : MonoBehaviour
{
    [SerializeField] private int bonus = 20;
    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }

    private void Awake()
    {
     
        AudioSource = GetComponent<AudioSource>();

        Assert.IsNotNull(clickSound);
        Assert.IsNotNull(AudioSource);
    }
        private void OnMouseDown()
    {
        AudioSource.PlayOneShot(clickSound);
        Invoke("AddBonusAndDelete", 1.0f) ;
    }

    private void AddBonusAndDelete()
    {
        GameManager.Instance.CurrentPlayer.AddXp(bonus);
        Destroy(gameObject);
    }



}
