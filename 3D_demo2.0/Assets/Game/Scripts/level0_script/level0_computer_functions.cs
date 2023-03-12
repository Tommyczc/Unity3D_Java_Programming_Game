using Michsky.DreamOS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level0_computer_functions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject boot_component;
    public GameObject computer_interface;
    public GameObject reboot_component;
    public GameObject quick_centre;
    public TMPro.TextMeshPro commandInput;
    public TMPro.TextMeshPro commandOutput;
    private bool openState=false;

    public bool computer_open
    {
        get { return computer_interface.activeSelf; }
    }

    void Start()
    {
        computer_interface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boot_computer()
    {
        computer_interface.gameObject.SetActive(true);
        reboot_component.GetComponent<RebootManager>().RunSystem();
        //boot_component.gameObject.GetComponent<BootManager>().start_boot();
    }

    


    public void shutdown_computer() 
    {
        reboot_component.GetComponent<RebootManager>().ShutdownSystem();
        quick_centre.GetComponent<PopupPanelManager>().ClosePanel();

    }
}
