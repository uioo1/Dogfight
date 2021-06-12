using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net;
using System;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using System.Threading;


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct packet{
    public char cmd;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string data;
}
public class zzzClient : MonoBehaviour
{  
    public string ip = "59.17.44.84";
    public int port;
    public string name;
    public InputField inputName;
    public InputField inputChat;
    public InputField inputRoomName;
    public InputField inputRoomPass;
    public InputField inputRoomPort;
    public InputField inputPass;
    IPEndPoint ipep;
    Socket serv;
    byte[] buffer = new byte[1025];
    private postbox p; 
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            sendChat();
            inputChat.text = "";
            inputChat.ActivateInputField();
        }
    }

    public void connector(){
        ipep = new IPEndPoint(IPAddress.Parse(ip), port);
        serv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        p = postbox.GetInstance;
        try{
            serv.Connect(ipep);
            serv.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, getPacket, this);
            
            name = inputName.text;
            int a = name.Length;
            
            byte[] b = new byte[1025];
            b[0] = 1;
            
            for(int i = 0; i < a; i++){
                b[i + 1] = (byte)name[i];
            }
            serv.Send(b);
        }
        catch (SocketException er){
            Debug.Log(er.Message.ToString());
        }
    }
    public void sendChat(){
        string data = inputChat.text.ToString();
        data = name + "\\" + data;
        sendPacket((char)1, data);
    }
    public void getRoom(){
        sendPacket((char)2, "");
    }
    public void makeRoom(){
        zzzServer s = gameObject.GetComponent<zzzServer>();
        zzzUDPClient c = gameObject.GetComponent<zzzUDPClient>();
        s.startServer(inputRoomPort.text);

        string myip = new WebClient().DownloadString("http://ipinfo.io/ip").Trim();
        c.ip = myip;
        c.port = int.Parse(inputRoomPort.text);
        c.openClient();
        SceneManager.LoadScene("MainScene");

        string data = myip + "\\" + inputRoomPort.text + "\\" + inputRoomName.text + "\\" + inputRoomPass.text;
        sendPacket((char)3, data);
    }

    public void accessRoom(int index){
        if(inputPass.text.Length == 0)
            sendPacket((char)4,  index + "\\" + "0");
        else
            sendPacket((char)4, index + "\\" + inputPass.text);
    }

    public void sendPacket(char cmd, string data){
        Encoding euckr = Encoding.UTF8;

        byte[] bytes = new byte[1025];
        int position = 0;

        byte[] sbuff = euckr.GetBytes(data);

        Buffer.BlockCopy(BitConverter.GetBytes(cmd), 0, bytes, position, sizeof(char));
        position = 1;
        for(int i = 0; i < sbuff.Length; i++){
            if(i == 1023){
                break;
            }
            bytes[position + i] = (byte)sbuff[i];
        }

        serv.Send(bytes);
    }

    private void getPacket(IAsyncResult result){
        int size = serv.EndReceive(result);
        packet temp = new packet();
        temp.cmd = (char)buffer[0];
        temp.data = Encoding.Default.GetString(buffer, 1, 1024);
        p.PushData(temp);
        
        serv.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, getPacket, this);
    }
}
