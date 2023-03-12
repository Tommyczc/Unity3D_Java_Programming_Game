using Google.Protobuf;
using Proto;
using System;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace javaCompiler
{
    public static class testJavaCompiler
    {
        public static Socket TcpClient;
        public static Thread tt;
        public static string tempMes="";
        static byte[] data = new byte[2048];
        public static string output="";
        public static string error="";
        private static string location = "";
        public static string theLocation { 
            get { return location; }
            set { if (value.Trim() == "medic" || value.Trim()=="frontdoor" || value.Trim()=="") { location = value; } }
        }
        public static System.Diagnostics.Process process=null;
        private static string savePath= Application.dataPath+ "/StreamingAssets/Java_server/javaServer.exe";
        public static Thread exeRunner;

        public static bool IsConnected() {
            //bool blockingState = TcpClient.Blocking;
            //try
            //{
            //    byte[] tmp = new byte[1];
            //    TcpClient.Blocking = false;
            //    TcpClient.Send(tmp, 0, 0);
            //                     return false;
            //}
            //catch (SocketException e)
            //{
            //    if (e.NativeErrorCode.Equals(10035))
            //        return false;
            //    else 
            //        return true;
            //}
            //finally
            //{
            //    TcpClient.Blocking = blockingState;    // 恢复状态
            //}
            return TcpClient.Connected;
        }

        static void ReceiveMessage()
        {

            while (TcpClient.Connected)
            {
                try
                {
                    int length = TcpClient.Receive(data);
                    if (length > 0)
                    {
                        Data recieve = Data.Parser.ParseFrom(data, 0, length);
                        string dataType = recieve.DataType;
                        string room = recieve.Room;
                        Google.Protobuf.Collections.MapField<string, string> info = recieve.Info;

                        if (dataType == "codeResult")
                        {
                            if (info.ContainsKey("output") && info.ContainsKey("error"))
                            {
                                //tempMes = info["output"] + "\n";
                                //tempMes += info["error"];
                                //Debug.Log(tempMes);
                                error = info["error"];
                                output = info["output"];
                                //return;
                            }
                        }

                        switch (room)
                        {
                            case "medic":
                                
                                if (dataType == "doorOpen")
                                {
                                    if (info.ContainsKey("numOfDoor"))
                                    {
                                        Debug.Log("the door is open: " + info["numOfDoor"]);
                                        tcpTerminal.operationByTerminal(room,dataType);
                                    }
                                }
                                break;

                            case "frontDoor":
                                if (dataType=="bugfixed") {
                                    if (info.ContainsKey("result")) {
                                        if (info["result"] == "true")
                                        {
                                            tcpTerminal.operationByTerminal(room, "bugfixed");
                                        }
                                        else { tcpTerminal.operationByTerminal(room, "bugNotfixed"); }
                                        Debug.Log(info["result"]);
                                    }
                                }
                                if (dataType == "doorOpen")
                                {
                                    if (info.ContainsKey("result"))
                                    {
                                        if (info["result"] == "open")
                                        {
                                            tcpTerminal.operationByTerminal(room, "open");
                                        }
                                        else if(info["result"] == "close") 
                                        {
                                            tcpTerminal.operationByTerminal(room, "close"); 
                                        }
                                        //Debug.Log("door open"+info["result"]);
                                    }
                                }
                                break;
                        }
                    }
                }
                catch (SocketException e)
                {
                    //Debug.Log(e.Message);
                    if (e.ErrorCode==10057) { Debug.Log("server shut down a connection"); break; }
                    
                }
                data = new byte[2048];
            }
        }

        public static void sendMes(byte[] pack) {
            TcpClient.Send(pack);
            //Debug.Log(pack.Length);
        }

        public static void ConnectToServer(string ip="127.0.0.1",int port=8888)
        {
            exeRunner = new Thread(delegate ()
            {
            RunExeByProcess(port.ToSafeString());
            });
            exeRunner.Start();

            //发起连接
            try
            {
                if (TcpClient!=null) {
                    TcpClient.Close();
                    TcpClient = null;
                }
                TcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                while (!IsConnected())
                //if(true)  //for debug
                {
                    try
                    {
                        TcpClient.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                    }
                    catch (SocketException e)
                    {
                        //Debug.Log(e.ErrorCode);
                        if (e.ErrorCode == 10061)
                        {
                            Debug.Log("server refuse connection");
                        }

                    }
                }

                tt = new Thread(ReceiveMessage);
                tt.Start();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);

            }

        }


        private static void RunExeByProcess(string port)
        {
            //open a new thread
            process = new System.Diagnostics.Process();

            //the name of exe file
            process.StartInfo.FileName = savePath;

            //set up the size of window
            //process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            //szet up the argumentg
            string arguments = port;
            process.StartInfo.Arguments = arguments;
            process.Start();
            process.WaitForExit(); //wait until the process terminated, a blocking mode
            
            
        }

        public static void closeconnect() {
            if (testJavaCompiler.IsConnected())
            {

                Data proto = new Data();  // create a message object instance
                proto.DataType = "command";  //input data
                proto.Room = "center";
                proto.Info.Add("command", "SystemOut");
                byte[] byteMes = proto.ToByteArray(); //serialize the data from object into bytecode

                sendMes(byteMes);
            }
            TcpClient.Close();
            process.Kill();
            exeRunner.Abort(); 

        }

        public static string GetLocalIp()
        {
            //得到本机名 
            string hostname = Dns.GetHostName();
            //解析主机名称或IP地址的system.net.iphostentry实例。
            IPHostEntry localhost = Dns.GetHostEntry(hostname);
            if (localhost != null)
            {
                foreach (IPAddress item in localhost.AddressList)
                {
                    //判断是否是内网IPv4地址
                    if (item.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return item.MapToIPv4().ToString();
                    }
                }
            }
            return "127.0.0.1";
        }
    }
}
