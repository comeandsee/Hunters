using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private  T instance;

    public  T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
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
