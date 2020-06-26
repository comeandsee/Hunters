using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

public class CaptureSceneManager : HuntersSceneManager
{
    private CaptureSceneStatus status = CaptureSceneStatus.InProgress;
    [SerializeField] private AudioClip clickExitSound;
    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    private AudioSource audioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(clickExitSound);
        Assert.IsNotNull(AudioSource);
    }
    private void Start()
    {
       // AnimalFactory.Instance.SelectedAnimal;
    }

    public CaptureSceneStatus Status
    {
        get { return status; }
    }
    public override void animalTapped(GameObject animal)
    {
        print("1 aniaml gartki ");
    }

    public override void playerTapped(GameObject player)
    {
        print("2 palyer gartki ");
    }

    public override void animalCollision()
    {
        status = CaptureSceneStatus.Successful;

        comeBackToWorldScene();

    }

    private void comeBackToWorldScene()
    {
        Animal[] animals = Resources.FindObjectsOfTypeAll<Animal>();
        foreach (Animal a in animals)
        {
            a.showObject();
        }


        IEnumerator coroutine = WaitAndGoToWorldScene(1.5f);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndGoToWorldScene(float waitTime)
    {
            yield return new WaitForSeconds(waitTime);
            SceneTransitionManager.Instance.GoToScene(HuntersConstants.SCENE_WORLD,
            new List<GameObject>());

    }

    public void ExitButtonClicked()
    {
        AudioSource.PlayOneShot(clickExitSound);
        status = CaptureSceneStatus.Failed;
        var animal = AnimalFactory.Instance.SelectedAnimal;
        if(animal)
        {
            animal.changePositionToDefault();
        }
        comeBackToWorldScene();
    }
}
