using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class calcel_function : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cancel_function() { 
        Destroy(this.gameObject);
    }

    public void setTimeScaleToOne()
    {
        Time.timeScale = 1;

    }
}
