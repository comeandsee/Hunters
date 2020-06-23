using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisControl : MonoBehaviour
{
    float delay = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
        Object.Destroy(gameObject, delay);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 1);

    }

    
}
