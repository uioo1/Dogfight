using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System;
using System.Runtime.InteropServices;
using System.Threading;



[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct packet{
    public char cmd;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string data;
}
public class zzzClient : MonoBehaviour
{
    // Start is called before the first frame update
    public string ip;
    public int port;
    public InputField inputName;
    public InputField inputNum;
    public InputField inputData;
    public Text outputNum;
    public Text outputData;
    IPEndPoint ipep;
    Socket serv;
    bool connected = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(connected){
            getPacket();
        }
    }

    public void connector(){
        ipep = new IPEndPoint(IPAddress.Parse(ip), port);
        serv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try{
            serv.Connect(ipep);
            Thread thread = new Thread (() => getPacket());
            
            byte[] bytes = new byte[1025];
            
            string name = inputName.text;
            int a = name.Length;
            
            bytes[0] = 1;
            
            for(int i = 0; i < a; i++){
                bytes[i + 1] = (byte)name[i];
            }
            serv.Send(bytes);
            thread.Start();
        }
        catch (SocketException er){
            Debug.Log(er.Message.ToString());
        }
    }

    public void sendPacket(){
        packet temp = new packet();
        temp.cmd = char.Parse(inputNum.text);
        temp.cmd -= (char)48;
        temp.data = inputData.text;
        int a = temp.data.Length;

        byte[] bytes = new byte[1025];
        int position = 0;
        Buffer.BlockCopy(BitConverter.GetBytes(temp.cmd), 0, bytes, position, sizeof(char));
        position = 1;
        for(int i = 0; i < a; i++){
            bytes[position + i] = (byte)temp.data[i];
        }

        serv.Send(bytes);
    }

    public void getPacket(){
        while(true){
            packet temp = new packet();
            byte[] recvBytes = new byte[1025];
            if(serv.Receive(recvBytes) <= 0){
                break;
            }
            
            temp.cmd = BitConverter.ToChar(recvBytes, 0);
            temp.data = Encoding.Default.GetString(recvBytes, 1, 1024);

            Debug.Log(temp.cmd);
            Debug.Log(temp.data);
        }
    }

}
