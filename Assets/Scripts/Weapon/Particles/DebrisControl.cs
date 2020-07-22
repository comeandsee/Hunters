using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisControl : MonoBehaviour
{
    float delay = 5.0f;
    float speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        //    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
       
      Object.Destroy(gameObject, delay);

    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindWithTag("Player");
        GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);

        transform.Rotate(0, 0, 1);

    }

    
}
