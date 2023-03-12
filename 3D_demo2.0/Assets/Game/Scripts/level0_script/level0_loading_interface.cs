using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level0_loading_interface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        this.gameObject.transform.SetSiblingIndex(3);// loading interface is at level 3
    }
}
