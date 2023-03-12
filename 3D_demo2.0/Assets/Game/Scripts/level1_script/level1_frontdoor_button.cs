using Google.Protobuf;
using javaCompiler;
using Michsky.UI.MTP;
using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level1_frontdoor_button : MonoBehaviour
{
    public string theText;
    private static bool solved = false;
    public GameObject warmingUI;
    public static bool isSolved { 
        get { return solved; }
        set { solved = value; }
    }
    public GameObject canvas;
    private Animator anim;
    private GameObject textObject;
    public GameObject frontDoor;
    private bool requestSending=false;
    public static bool doorOpening=false;


    // Start is called before the first frame update
    void Start()
    {
        anim = canvas.GetComponent<Animator>();
        anim.SetInteger("UI_open", 0);
        canvas.SetActive(false);


        Transform[] myTransforms = canvas.gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in myTransforms)
        {
            if (child.name == "Text")
            {
                textObject = child.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    { }

    

    private void OnTriggerEnter(Collider other)
    {
        if (textObject != null)
        {
            if (textObject.GetComponent<TextItem>().text != theText)
            {
                textObject.GetComponent<TextItem>().text = theText;
                textObject.GetComponent<TextItem>().UpdateText();
            }
            canvas.SetActive(true);
            anim.SetInteger("UI_open", 1);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isSolved != true)
            {
                if (warmingUI.GetComponent<InterfaceAnimManager>().currentState== CSFHIAnimableState.disappeared) { StartCoroutine(showWarningUI()); }// this is a kind of checking anim state of UI
            }
            else if(isSolved && !requestSending) {

                StartCoroutine(sendDoorRequest());

            }
        }
    }

    private IEnumerator sendDoorRequest() {
        string code = "pressButton();";
        send_code(code);
        requestSending = true;
        yield return new WaitForSeconds(1f);
        requestSending=false;
    }

    public void openFrontDoor() {
        frontDoor.gameObject.GetComponent<level1_frontDoor_steamfx>().playSteamfx();
        frontDoor.gameObject.GetComponent<Animator>().SetBool("open",true);
        frontDoor.gameObject.GetComponent<Animator>().SetFloat("speed", 1f);
        AnimatorStateInfo animatorInfo= frontDoor.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.normalizedTime>0.99f) {
            Debug.Log("finish");
            frontDoor.gameObject.GetComponent<level1_frontDoor_steamfx>().pauseSteamfx();
        }
    }

    public void closeFrontDoor()
    {
        frontDoor.gameObject.GetComponent<level1_frontDoor_steamfx>().playSteamfx();
        frontDoor.gameObject.GetComponent<Animator>().SetBool("open", true);
        frontDoor.gameObject.GetComponent<Animator>().SetFloat("speed", -1f);
        AnimatorStateInfo animatorInfo = frontDoor.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.normalizedTime >= 1.0f)
        {
            frontDoor.gameObject.GetComponent<level1_frontDoor_steamfx>().pauseSteamfx();
        }
    }

    private IEnumerator showWarningUI() {
        warmingUI.GetComponent<InterfaceAnimManager>().startAppear();
        yield return new WaitForSeconds(1.5f);
        warmingUI.GetComponent<InterfaceAnimManager>().startDisappear();
        yield return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (textObject != null)
        {
            if (textObject.GetComponent<TextItem>().text != theText)
            {
                textObject.GetComponent<TextItem>().text = theText;
                textObject.GetComponent<TextItem>().UpdateText();
            }
            if (anim.GetInteger("UI_open") != 2)
            {
                anim.SetInteger("UI_open", 2);
            }

            //canvas.SetActive(false);

        }
    }
    private void send_code(string javaCode)
    {
        string location = testJavaCompiler.theLocation;
        if (testJavaCompiler.IsConnected())
        {
            Data proto = new Data();
            proto.DataType = "javaCode";
            proto.Room = "frontdoor";
            proto.Info.Add("code", javaCode);
            byte[] byteMes = proto.ToByteArray();
            testJavaCompiler.sendMes(byteMes);
        }
        else { Debug.Log("show does not connected"); }
    }
}
