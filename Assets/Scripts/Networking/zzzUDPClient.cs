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

public class zzzUDPClient : MonoBehaviour
{
    // Start is called before the first frame update
    public string ip;
    public int port;
    
    bool isPlayer1;
    Socket serv = null;
    postboxUdp post = postboxUdp.GetudpClientInstance;
    byte[] buffer = new byte[2048];
    public static object RawDeserializeEx( byte[] rawdatas, Type anytype )
    {
        int rawsize = Marshal.SizeOf( anytype );
        if( rawsize > rawdatas.Length )
            return null;
        GCHandle handle = GCHandle.Alloc( rawdatas, GCHandleType.Pinned );
        IntPtr buffer = handle.AddrOfPinnedObject();
        object retobj = Marshal.PtrToStructure( buffer, anytype );
        handle.Free();
        return retobj;
    }

    public static byte[] RawSerializeEx( object anything )
    {
        int rawsize = Marshal.SizeOf( anything );
        byte[] rawdatas = new byte[ rawsize ];
        GCHandle handle = GCHandle.Alloc( rawdatas, GCHandleType.Pinned );
        IntPtr buffer = handle.AddrOfPinnedObject();
        Marshal.StructureToPtr( anything, buffer, false );
        handle.Free();
        return rawdatas;
    }





    public void openClient(){
        IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, port);
        serv = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        serv.Connect(ep);
        sendPacket((char)1, name);
        Debug.Log("Client : sendPacket to " + ip + " : " + port);
        serv.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, getUDPPacket, this);
    }
    private void getUDPPacket(IAsyncResult result){
        int size = serv.EndReceive(result);
        UdpPack temp = new UdpPack();
        temp = (UdpPack)zzzUDPClient.RawDeserializeEx(buffer, typeof(UdpPack));
        post.PushData(temp);
        serv.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, getUDPPacket, this);
    }
    public void sendPacket(char cmd, string data){
        UdpPack pp = new UdpPack();
        pp.cmd = cmd;
        pp.transform = null;
        pp.isfliped = true;
        pp.textString = data;

        Debug.Log(serv.Send(RawSerializeEx(pp)));
    }
    public void sendTransform(Transform data, bool flip){
        UdpPack pp = new UdpPack();
        pp.cmd = 4;
        pp.transform = data;
        pp.isfliped = flip;
        pp.textString = "";

        serv.Send(RawSerializeEx(pp));
    }
    public void ResponseData(UdpPack d)
    {
        switch ((int)d.cmd)
        {
            case 0 : 
                serv.Close();
                //unconnected
            break;
            case 1 :
                isPlayer1 = true;
                //connected
            break;
            case 2 :
                isPlayer1 = false;
                //connected
            break;
            case 3 :
                //countdown
            break;
            case 4 :
                //renew Position
            break;
            case 5 : 
                // you win
            break;
            case 6 :
                // you lose
            break;
            case 7 : 
                // game set;
            break;

            default:
            break;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
    }
    void FixedUpdate()
    {
        UdpPack data = post.GetData();
        //우편함에 데이타가 있는 경우
        if (!data.Equals(new UdpPack()))
        {
            //데이타로 UI 갱신
            ResponseData(data);
        }
    }
}
