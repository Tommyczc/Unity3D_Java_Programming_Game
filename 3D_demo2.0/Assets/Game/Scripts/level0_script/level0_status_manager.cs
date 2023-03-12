using javaCompiler;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level0_status_manager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private string currentSceneName;
    [Header("my panel")]
    [SerializeField]
    public Text health_value;
    [SerializeField]
    public Text oxygen_value;

    [Header("system")]
    [SerializeField]
    public TextMeshProUGUI systemStatus;
    public TextMeshProUGUI checkButtonTitle;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player==null) {
            currentSceneName = SceneManager.GetActiveScene().name;
            player = GameObject.Find("Stylized Astronaut");
        }

        oxygen_value.text =player.GetComponent<Health_System>().oxygen_remain.ToString()+" / "+ player.GetComponent<Health_System>().oxygen_max.ToString();
        health_value.text = player.GetComponent<Health_System>().HP + " / 100";


        if (testJavaCompiler.IsConnected())
        {
            systemStatus.text = "<color=green>connected</color>";
            checkButtonTitle.text = "Unconnect";
        }
        else { systemStatus.text = "<color=red>unconnected</color>"; checkButtonTitle.text = "Reconnect"; }
    }

    public void buttonForstatu() {
        if (testJavaCompiler.IsConnected())
        {
            closeconnectServer();
            
        }
        else { 
            reconnectServer();
        }
    }

    public void closeconnectServer() {
        testJavaCompiler.closeconnect();
    }

    public void reconnectServer() {
        testJavaCompiler.ConnectToServer("127.0.0.1", Int32.Parse(level0_load_scene.port_number));
    }
}
