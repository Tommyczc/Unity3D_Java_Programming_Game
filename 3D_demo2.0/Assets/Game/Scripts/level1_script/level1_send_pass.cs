using Google.Protobuf;
using javaCompiler;
using Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace level.one
{
    public class level1_send_pass : MonoBehaviour
    {
        // Start is called before the first frame update

        public InputField the_com;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void send()
        {
            if (testJavaCompiler.IsConnected())
            {
                Data proto = new Data();
                proto.DataType = "checkPass";
                proto.Room = testJavaCompiler.theLocation;
                proto.Info.Add("pass", the_com.text);
                byte[] byteMes = proto.ToByteArray();
                testJavaCompiler.sendMes(byteMes);
            }
            else { Debug.Log("show does not connected"); }
        }
    }
}
