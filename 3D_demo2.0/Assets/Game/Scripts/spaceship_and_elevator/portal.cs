using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public GameObject destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        other.gameObject.transform.position = destination.gameObject.transform.position;
        //Debug.Log("hello");
        //other.gameObject.transform.Translate(destination.gameObject.transform.position);
    }
}
