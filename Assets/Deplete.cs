using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deplete : MonoBehaviour
{
    // Start is called before the first frame update
    public int resourceHP = 100;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(resourceHP < 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        resourceHP -= 1;
    }
}
