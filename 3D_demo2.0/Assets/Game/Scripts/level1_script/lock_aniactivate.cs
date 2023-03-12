using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lock_aniactivate : MonoBehaviour
{

    public GameObject lock_object;
    public bool openState=true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) {

        if (openState) {
            lock_object.GetComponent<InterfaceAnimManager>().startAppear();
        }
    
    }

    private void OnTriggerStay(Collider other) {

        if (openState==false) {
            lock_object.GetComponent<InterfaceAnimManager>().startDisappear();
        }
    }

    private void OnTriggerExit (Collider other){
        lock_object.GetComponent<InterfaceAnimManager>().startDisappear();
    }
}
