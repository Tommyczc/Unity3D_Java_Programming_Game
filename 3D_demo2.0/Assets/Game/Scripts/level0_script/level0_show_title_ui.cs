using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level0_show_title_ui : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject title_ui;
    void Start()
    {
        title_ui = GameObject.Find("loadScene");
        title_ui.GetComponent<level0_load_scene>().show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
