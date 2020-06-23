using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    private AsyncOperation scenceAsync;

    public void GoToScene(string scenename, List<GameObject> objectsToMove)
    {
        StartCoroutine(LoadScene(scenename, objectsToMove));
     }

    private IEnumerator LoadScene(string scenename, List<GameObject> objectsToMove)
    {
        SceneManager.LoadSceneAsync(scenename);

        SceneManager.sceneLoaded += (newScene, mode) => {
            SceneManager.SetActiveScene(newScene);
        };

        Scene sceneToLoad = SceneManager.GetSceneByName(scenename);
        foreach(GameObject obj in objectsToMove)
        {
            SceneManager.MoveGameObjectToScene(obj, sceneToLoad);
        }

        yield return null;
    }
}
