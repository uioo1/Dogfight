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

public class zzzServer : MonoBehaviour
{
    // Start is called before the first frame update
    byte[] buffer = new byte[2048];
    Socket socket;
    Socket other = null;
    Socket playerOther = null;
    Socket playerOther2 = null;
    string otherName;
    postboxUdp pUDP = postboxUdp.GetUdpInstance;

    int counter = 0;
    int counter2 = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UdpPack data = pUDP.GetData();
        if(playerOther != null){
            counter++;
        }
        if(playerOther2 != null){
            counter2++;
        }
        if(counter > 100){
            sendPacket((char)7, "", playerOther);
            playerOther = null;
            counter = 0;
        }
        if(counter2 > 100){
            sendPacket((char)7, "", playerOther2);
            playerOther2 = null;
            counter = 0;
        }
            //우편함에 데이타가 있는 경우
        if (!data.Equals(new UdpPack()))
        {
            //데이타로 UI 갱신
            ResponseData(data);
            counter = 0;
        }
    }

    public void ResponseData(UdpPack d)
    {
        Debug.Log("Udp Server : "+ ((int)d.cmd).ToString());
        switch ((int)d.cmd)
        {
            case 1 :
                otherName = d.textString;
                sendConnectionACK();
            break;
            

            default:
            break;
        }
    }
    public void sendConnectionACK(){
        if(playerOther == null){
            playerOther = other;
            playerOther.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, getUDPPacket, this);
            sendPacket((char)1, "", other);
        }
        else if (playerOther2 == null){
            playerOther2 = other;
            playerOther2.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, getUDPPacket, this);
            sendPacket((char)2, "", other);
        }
        else {
            sendPacket((char)0 , "", other);
            other.Close();
        }
    }

    public void sendStartCounter(){
        sendPacket((char)3, "", playerOther);
        sendPacket((char)3, "", playerOther2);
    }

    public void startServer(string port){
        try{
            IPEndPoint ipEndPoint =new IPEndPoint(IPAddress.Any, int.Parse(port));
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Bind(ipEndPoint);
            socket.Listen(10);
            socket.BeginAccept(getAccept, this);
            Debug.Log("start : " + ipEndPoint.ToString());
        }
        catch (SocketException er){
            Debug.Log(er.Message.ToString());
        }
    }

    public void sendPacket(char cmd, string data, Socket other){
        UdpPack pp = new UdpPack();
        pp.cmd = cmd;
        pp.transform = null;
        pp.isfliped = true;
        pp.textString = data;

        other.Send(zzzUDPClient.RawSerializeEx(pp));
    }
    public void sendTransform(Transform data, bool flip, Socket other){
        UdpPack pp = new UdpPack();
        pp.cmd = 4;
        pp.transform = data;
        pp.isfliped = flip;
        pp.textString = "";
        
        other.Send(zzzUDPClient.RawSerializeEx(pp));
    }
    private void getAccept(IAsyncResult result){
        Debug.Log("someone in!");
        other = socket.EndAccept(result);
        sendConnectionACK();
    }
    private void getUDPPacket(IAsyncResult result){
        int size = socket.EndReceive(result);
        UdpPack temp = new UdpPack();
        temp = (UdpPack)zzzUDPClient.RawDeserializeEx(buffer, typeof(UdpPack));
        Debug.Log("Udp Server : "+ ((int)temp.cmd).ToString());
        pUDP.PushData(temp);
        
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, getUDPPacket, this);
    }
}
