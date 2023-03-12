using Google.Protobuf;
using javaCompiler;
using Proto;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bugfix_button : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject desktopButton;
    public Button runUI;
    //public TMP_InputField Methodanswer;
    public TextMeshProUGUI Methodanswer;
    public TextMeshProUGUI info;
    string currentInput = "";
    void Start()
    {
        desktopButton.gameObject.SetActive(false);
        runUI.onClick.AddListener(
            delegate ()
            {
                if (Methodanswer.text.Trim() == "" 
                || !(Methodanswer.text.Trim().Contains("void") 
                && Methodanswer.text.Trim().Contains("pressButton")) 
                || !Methodanswer.text.Remove(Methodanswer.text.Length - 1).Trim().EndsWith("}"))
                {
                    info.text = "Please define a void method named pressButton()";
                }
                else {
                    send_code(Methodanswer.text.Remove(Methodanswer.text.Length-1)) ;
                }//send the message to the server
            }
        );
    }

    private void send_code(string javaCode)
    {
        string location = testJavaCompiler.theLocation;
        if (testJavaCompiler.IsConnected())
        {
            Data proto = new Data();
            proto.DataType = "bugfix";
            proto.Room = "frontdoor";
            //proto.Info.Add("code", java_highlighting.getPureText(javaCode));
            proto.Info.Add("code", javaCode);
            byte[] byteMes = proto.ToByteArray();
            testJavaCompiler.sendMes(byteMes);
        }
        else { Debug.Log("show does not connected"); }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (testJavaCompiler.theLocation == "frontdoor" )
        {
            desktopButton.gameObject.SetActive(true);
            //Debug.Log(testJavaCompiler.theLocation);
        }
        else if (testJavaCompiler.theLocation != "frontdoor")
        {
            desktopButton.gameObject.SetActive(false);
            //Debug.Log(testJavaCompiler.theLocation);
        }

        if (tcpTerminal.frontDoor_bugfix==true) {
            info.text = "<color=\"green\">Bingo! the method has been defined sussesfully</color>";
        }

#if false
        if (Methodanswer.text != currentInput)
        {
            java_highlighting.updateDuplicateTime(Methodanswer.text);
            int wordHighLight = java_highlighting.getKeyWordDuplicateTime(Methodanswer.text);
            int integerHighLight = java_highlighting.getIntegerDuplicateTime(Methodanswer.text);
            if (wordHighLight != java_highlighting.wordHighLight || integerHighLight != java_highlighting.integerHighLight)
            {
                Methodanswer.text = java_highlighting.highlighting(Methodanswer.text);
                currentInput = Methodanswer.text;
                if (wordHighLight != java_highlighting.wordHighLight)
                {
                    /*
                    input_code.caretPosition = input_code.text.Length;
                    if (char.IsLetter(input_code.text[input_code.caretPosition])) { input_code.caretPosition += 1; }
                    else { Debug.Log(input_code.text[input_code.caretPosition+java_highlighting.keyWordColor.Length]); }
                    */
                    StartCoroutine(updateHighLight());

                }
                else
                {
                    Methodanswer.caretPosition += 1;
                }
                /*
                Debug.Log("java update: "+ java_highlighting.wordHighLight+" "+ java_highlighting.integerHighLight);
                Debug.Log("scr update: "+ wordHighLight+" "+ integerHighLight);
                */
            }

        }
#endif

    }

#if false
    private IEnumerator updateHighLight()
    {
        int i = 0;
        while (char.IsLetter(Methodanswer.text[Methodanswer.caretPosition + java_highlighting.keyWordColor.Length + i]))
        {
            i++;
        }
        Methodanswer.caretPosition = java_highlighting.keyWordColor.Length + i + java_highlighting.colorFinishWord.Length;
        yield return null;
    }
#endif
}
