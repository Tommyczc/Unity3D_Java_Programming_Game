using javaCompiler;
using Michsky.DreamOS;
using Michsky.UI.ModernUIPack;
using Michsky.UI.MTP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level0_load_scene : MonoBehaviour
{
    [Header("title begin ui")]
    [SerializeField]
    public GameObject title_ui;
    [SerializeField]
    public GameObject main_text1;
    [SerializeField]
    public GameObject main_text2;
    [SerializeField]
    public GameObject sub_text;
    [SerializeField]
    private Animator title_ani;

    [Header("loading interface")]
    [SerializeField]
    public GameObject theloading;
    [SerializeField]
    private GameObject progrogress_bar;

    [Header("setting and start page")]
    [SerializeField]
    public GameObject Setting;
    [SerializeField]
    public GameObject level0_menu;
    [SerializeField]
    public GameObject audioManager;
    [SerializeField]
    public Text Ip_text;
    [SerializeField]
    public InputField port_input;
    [SerializeField]
    public static string port_number;

    [Header("task manager")]
    [SerializeField]
    public GameObject taskpanel;

    // Start is called before the first frame update
    void Start()
    {

        audioManager.GetComponent<AudioManager>().playAudio("Audio_level0_bgm1",0.4f);
        //title_ui.GetComponent<StyleManager>().InitializeSpeed(0.5f);
        Ip_text.text = "IP: " +testJavaCompiler.GetLocalIp();

        title_ani=title_ui.GetComponent<Animator>();
        title_ui.SetActive(false);
        progrogress_bar= theloading.transform.Find("Progress_bar").gameObject;
        taskpanel.GetComponent<Michsky.UI.ModernUIPack.NotificationManager>().enableTimer = false;
        theloading.SetActive(false);

        
        //theloading = (GameObject)Resources.Load("Prefabs/loading_interface");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Setting.GetComponent<setting>().showUp();
        }
    }

    public void loadscene(string level) {
        if (SceneManager.GetActiveScene().name.ToLower()==level.ToLower()) { return; }
        audioManager.GetComponent<AudioManager>().turnOff();
        if (level == "level1") {
            
            //title_ui.SetActive(false);
            if (main_text1.GetComponent<TextItem>().text != "Chapter One")
            {
                main_text1.GetComponent<TextItem>().text = "Chapter One";
                main_text1.GetComponent<TextItem>().UpdateText();
            }
            if (main_text2.GetComponent<TextItem>().text != "Chapter One")
            {
                main_text2.GetComponent<TextItem>().text = "Chapter One";
                main_text2.GetComponent<TextItem>().UpdateText();
            }
            if (sub_text.GetComponent<TextItem>().text != "Truth")
            {
                sub_text.GetComponent<TextItem>().text = "Truth";
                sub_text.GetComponent<TextItem>().UpdateText();
            }

        }

        else if(level=="level0"){
            /*
            testJavaCompiler.TcpClient.Close();
            testJavaCompiler.process.Close();
            testJavaCompiler.exeRunner.Abort();
            */
            testJavaCompiler.closeconnect();
            StartCoroutine(loadLevelAsync(level));
        }
        
    }

    public void show() {
        StartCoroutine(showtitle_UI());
    }

    private IEnumerator showtitle_UI() {


        title_ui.SetActive (true);
        //yield return new WaitForSeconds(1.0f);
        //title_ui.GetComponent<StyleManager>().PlayIn();
        title_ani.SetBool("show",true);
        yield return new WaitForSeconds(4.0f);
        title_ani.SetBool("show", false);
        //yield return new WaitForSeconds(1.5f);
        title_ui.SetActive(false);
        //title_ui.GetComponent<StyleManager>().PlayOut();
    }

    private IEnumerator loadLevelAsync(string name) {
        theloading.SetActive(true);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        
        float progress = 0f;
        while (progress!=100f) {
            //float progress = Mathf.Lerp(0f,1f,operation.progress);
            progrogress_bar.GetComponent<ProgressBar>().ChangeValue(progress);
            //Debug.Log(progress);
            yield return new WaitForSeconds(0.01f);
            progress += 1f;
        }
        if (operation.isDone) {

            progrogress_bar.GetComponent<ProgressBar>().ChangeValue(100f);
            yield return new WaitForSeconds(1.0f);
            
        }

        
        theloading.SetActive(false);
        

        yield return new WaitForSeconds(0.3f);
        if (name != "level0") {
            StartCoroutine(showtitle_UI()); showTaskPanel(1f, 10f, "Task 1", "Find the Password and Escape the Room!") ; Cursor.lockState = CursorLockMode.Confined;
            if (name=="level1") {
                audioManager.GetComponent<AudioManager>().playAudio("Audio_level0_bgm3",0.2f);
            }
        }
        else {
            
            level0_menu.SetActive(true);
            audioManager.GetComponent<AudioManager>().playAudio("Audio_level0_bgm1",0.3f);
        }
    }

    public void load_Level1()
    {
        tcpTerminal.updateAllState();
        loadscene("level1");
        port_number = port_input.text;
        testJavaCompiler.ConnectToServer("127.0.0.1", Int32.Parse(port_number));
        if (testJavaCompiler.IsConnected())
        {
            StartCoroutine(loadLevelAsync("level1"));
        }
        else
        {
            Debug.Log("no connect");
        }
    }

    public void load_Level2() { }

    public void load_Level0() {
        Setting.GetComponent<setting>().showDown();
        
        loadscene("level0");
    }

    public void quit_menu() {
        Application.Quit();
    }

    public void OnApplicationQuit()
    {
        /*
        testJavaCompiler.TcpClient.Close();
        testJavaCompiler.process.Close();
        testJavaCompiler.exeRunner.Abort();
        */
        testJavaCompiler.closeconnect();
    }

    public void game_interrupt() {
        Time.timeScale = 0;
    }
    public void game_start() {
        Time.timeScale = 1;
    }

    public void reLoadScene() {
        if (SceneManager.GetActiveScene().name == "level0") {
            load_Level0();
        }
        else if (SceneManager.GetActiveScene().name == "level1") { 
            load_Level1();
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            //load_Level2();
        }
    }

    public void showTaskPanel(float waittime, float executetime, string title, string description) {
        taskpanel.GetComponent<Michsky.UI.ModernUIPack.NotificationManager>().title = title;
        taskpanel.GetComponent<Michsky.UI.ModernUIPack.NotificationManager>().description= description;
        taskpanel.GetComponent<Michsky.UI.ModernUIPack.NotificationManager>().UpdateUI();
        StartCoroutine(showTask(waittime, executetime));
    }

    private IEnumerator showTask(float beforetime, float aftertime) {
        yield return new WaitForSeconds(beforetime);
        taskpanel.GetComponent<Michsky.UI.ModernUIPack.NotificationManager>().OpenNotification();
        yield return new WaitForSeconds(aftertime);
        taskpanel.GetComponent<Michsky.UI.ModernUIPack.NotificationManager>().CloseNotification();
    }

    void Awake()
    {

    }
}
