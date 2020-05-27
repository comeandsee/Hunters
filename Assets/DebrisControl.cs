using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      //  GetComponent<Rigidbody>().AddForce(transform.forward * -5, ForceMode.Impulse);
          GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);
        // Vector3 desiredDirection = new Vector3(0, 0, -1); // set this to the direction you want.
        // GetComponent<Rigidbody>().velocity = desiredDirection.normalized * GetComponent<Rigidbody>().velocity.magnitude;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 1);
       // GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -5);

    }
}
