using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level0_dont_des : MonoBehaviour
{
    private static bool isExist=false;
    
    // Start is called before the first frame update
    void Start()
    {
        checkRepeated();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void checkRepeated()
    {
        if (isExist == false)
        {
            isExist = true;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }
    }

    void Awake()
    {
        
    }
}
