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
    byte[] buffer = new byte[1024];
    Socket socket;
    EndPoint other = null;
    EndPoint playerOther = null;
    string otherName;
    postbox pUDP = postbox.GetUdpInstance;

    int counter = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        packet data = pUDP.GetData();
        if(playerOther != null){
            counter++;
        }
        if(counter > 100){
            playerOther = null;
            counter = 0;
        }
            //우편함에 데이타가 있는 경우
        if (!data.Equals(new packet()))
        {
            //데이타로 UI 갱신
            ResponseData(data);
            counter = 0;
        }
    }

    public void ResponseData(packet d)
    {
        Debug.Log((int)d.cmd);
        Debug.Log(d.data);
        switch ((int)d.cmd)
        {
            case 1 :
                otherName =  d.data;
                sendConnectionACK();
            break;
            

            default:
            break;
        }
    }
    public void sendConnectionACK(){
        if(playerOther == null){
            playerOther = other;
            sendUDPPacket((char)1, "");
        }
        else {
            sendUDPPacket((char)2, "");
        }
    }

    public void sendStartCounter(){
        sendUDPPacket((char)3, "");
    }

    public void startServer(string port){
        other = new IPEndPoint(IPAddress.Any, 0);
        Debug.Log(int.Parse(port));
        IPEndPoint ipEndPoint =new IPEndPoint(IPAddress.Any, int.Parse(port));
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipEndPoint);
        
        socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref other, getUDPPacket, this);
    }

    public void sendUDPPacket(char cmd, string data){
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

        socket.SendTo(bytes, other);
    }

    private void getUDPPacket(IAsyncResult result){
        int size = socket.EndReceiveFrom(result, ref other);
        packet temp = new packet();
        temp.cmd = (char)buffer[0];
        temp.data = Encoding.Default.GetString(buffer, 1, 1024);
        Debug.Log(size);
        pUDP.PushData(temp);
        
        socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref other, getUDPPacket, this);
    }
}
