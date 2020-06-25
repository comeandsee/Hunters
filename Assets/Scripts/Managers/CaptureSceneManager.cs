using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CaptureSceneManager : HuntersSceneManager
{
    private CaptureSceneStatus status = CaptureSceneStatus.InProgress;

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

    public override void animalCollision(GameObject animal, Collider other)
    {
        status = CaptureSceneStatus.Successful;
        IEnumerator coroutine = WaitAndGoToWorldScene(1.5f);
        StartCoroutine(coroutine);

        

    }

    private IEnumerator WaitAndGoToWorldScene(float waitTime)
    {
            yield return new WaitForSeconds(waitTime);
            SceneTransitionManager.Instance.GoToScene(HuntersConstants.SCENE_WORLD,
            new List<GameObject>());

    }
}
