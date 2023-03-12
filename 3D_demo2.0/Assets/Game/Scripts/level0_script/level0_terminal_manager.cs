using Google.Protobuf;
using javaCompiler;
using Proto;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class level0_terminal_manager : MonoBehaviour
{
    // Start is called before the first frame update
    //public TMP_InputField input_code;
    public TextMeshProUGUI input_code;
    public TextMeshProUGUI output_code;
    public Button confirm;
    private string outputColorBegin = "<color=#87ceff>";
    private string outputColorEnd = "</color>";
    private string currentLocation="";
    private string currentInput = "";

    void Start()
    {
        
        if (testJavaCompiler.output.Trim() != "" || testJavaCompiler.output.Trim() != "") {
            testJavaCompiler.output = testJavaCompiler.output = "";
        }
        //input_code.text = "";
        //output_code.text = outputColorBegin+"Current Location: " + testJavaCompiler.theLocation + "\n" + "Output>\n"+outputColorEnd;
        confirm.onClick.AddListener(
            delegate () {
                if (input_code.text.Trim() == "") { return; }
                else {
                    send_code(input_code.text.Remove(input_code.text.Length - 1));
                    //input_code.text = "";
                    output_code.text = "";
                }
            }
            );
    }

    // Update is called once per frame
    void Update()
    {
        /*   
           if (input_code.text!=currentInput) {
               java_highlighting.updateDuplicateTime(input_code.text);
               int wordHighLight=java_highlighting.getKeyWordDuplicateTime(input_code.text);
               int integerHighLight=java_highlighting.getIntegerDuplicateTime(input_code.text);
               if (wordHighLight != java_highlighting.wordHighLight || integerHighLight != java_highlighting.integerHighLight)
               {
                   input_code.text = java_highlighting.highlighting(input_code.text);
                   currentInput = input_code.text;
                   if (wordHighLight != java_highlighting.wordHighLight)
                   {

                       //input_code.caretPosition = input_code.text.Length;
                       //if (char.IsLetter(input_code.text[input_code.caretPosition])) { input_code.caretPosition += 1; }
                       //else { Debug.Log(input_code.text[input_code.caretPosition+java_highlighting.keyWordColor.Length]); }

                       StartCoroutine(updateHighLight());

                   }
                   else {
                       input_code.caretPosition +=1;
                   }
               }
           }
           */

        if (testJavaCompiler.output.Trim() != "" || testJavaCompiler.error.Trim() != "") {
            output_code.text = outputColorBegin+"Current Location: " + testJavaCompiler.theLocation + "\n" + "Output>\n"+outputColorEnd + testJavaCompiler.output;
            if (testJavaCompiler.error.Trim() != "")
            {
                output_code.text += "<color=red>" + "\nError>\n" + testJavaCompiler.error + "</color>";
                testJavaCompiler.error = "";
                testJavaCompiler.output = "";
            }
            else {
                testJavaCompiler.output = "";
            }
        }

        else if (currentLocation!=testJavaCompiler.theLocation) {
            currentLocation = testJavaCompiler.theLocation;
            output_code.text = "Current Location: " + testJavaCompiler.theLocation + "\n" + "Output>\n";
        }


    }

#if false
    private IEnumerator updateHighLight() {
        int i = 0;
        //Debug.Log(input_code.text[input_code.caretPosition + java_highlighting.keyWordColor.Length + i]);
        if (input_code.text[input_code.caretPosition + java_highlighting.keyWordColor.Length + 1] == ';')
        {
            
            input_code.caretPosition += java_highlighting.keyWordColor.Length + 1 + java_highlighting.colorFinishWord.Length;
        }

        while (char.IsLetter(input_code.text[input_code.caretPosition + java_highlighting.keyWordColor.Length + i]))
        {
            i++;
        }
        input_code.caretPosition += java_highlighting.keyWordColor.Length + i + java_highlighting.colorFinishWord.Length;

        //Debug.Log(input_code.text[input_code.caretPosition ]);

        yield return null;
    }
#endif

    private void send_code(string javaCode) {
        string location=testJavaCompiler.theLocation;
        if (testJavaCompiler.IsConnected())
        {

            Data proto = new Data();  // create a message object instance
            proto.DataType = "javaCode";  //input data
            proto.Room = location;
            proto.Info.Add("code", javaCode);
            byte[] byteMes = proto.ToByteArray(); //serialize the data from object into bytecode

            testJavaCompiler.sendMes(byteMes);
        }
        else {
            testJavaCompiler.error = "Sorry, the connection between the server is closed";
        }
    }

    public void clean_all() {
        if (testJavaCompiler.output != "" || testJavaCompiler.output != "")
        {
            testJavaCompiler.output = "";
            testJavaCompiler.error = "";
            output_code.text = "";
            input_code.text = "";
        }
    }

    void Awake()
    {
        output_code.text = outputColorBegin + "Current Location: " + testJavaCompiler.theLocation + "\n" + "Output>\n" + outputColorEnd;
    }
}
