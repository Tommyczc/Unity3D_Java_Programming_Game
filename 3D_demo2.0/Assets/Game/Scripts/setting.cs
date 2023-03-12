using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Michsky.UI.ModernUIPack;

public class setting : MonoBehaviour
{
    public Slider MainSoundBar;
    public Slider BGMSoundBar;
    public Slider MouseHorizontalSpeed;
    public Slider UISoundBar;
    public Toggle screenToggle;
    public GameObject applyButton;
    public Button goBack;
    public GameObject audio_manager;
    public GameObject computer_interface;
    

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetSiblingIndex(2);
        screenToggle.isOn= !Screen.fullScreen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showUp() {
        if (computer_interface.activeSelf != true)
        {
            if (SceneManager.GetActiveScene().name.ToLower() == "level0")
            {
                this.gameObject.SetActive(true);
                goBack.transform.gameObject.SetActive(false);
            }
            else { this.gameObject.SetActive(true); goBack.transform.gameObject.SetActive(true); }
            Time.timeScale = 0;
        }
    }

    public void showDown() {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainSoundValueEdit() {
        audio_manager.GetComponent<AudioManager>().SetMasterVolume(MainSoundBar.value);
    }

    public void BGMSoundValueEdit()
    {
        audio_manager.GetComponent<AudioManager>().SetBGMVolume(BGMSoundBar.value);
    }

    public void UISoundValueEdit()
    {
        audio_manager.GetComponent<AudioManager>().SetUIVolume(UISoundBar.value);
    }

    public void MouseSpeedChange()
    {
        Player.mouseXSpeed = MouseHorizontalSpeed.value;
    }
    public void screenSize() {
        if (screenToggle.isOn) { Debug.Log("on"); Screen.fullScreen = false; }//windowed
        else { 
            Debug.Log("off"); 
            Resolution[] resolutions = Screen.resolutions;
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
            Screen.fullScreen = true;
        }//full screen
    }

    void Awake()
    {
        this.gameObject.transform.SetSiblingIndex(2); //setting interface is at level 2
    }
}
