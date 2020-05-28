using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static  T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            DontDestroyOnLoad(instance);
            return instance;
        }
    }
    /*
    private void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }*/
}
