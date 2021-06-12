using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class postboxUdp {
    private static postboxUdp udpInstance;
    private static postboxUdp udpClientInstance;
    //싱글턴 인스턴스 반환
    public static postboxUdp GetUdpInstance
    {
        get
        {
            if (udpInstance == null)
                udpInstance = new postboxUdp();
 
            return udpInstance;
        }
    }

    public static postboxUdp GetudpClientInstance
    {
        get
        {
            if (udpClientInstance == null)
                udpClientInstance = new postboxUdp();
 
            return udpClientInstance;
        }
    }
    //데이타를 담을 큐
    private Queue<UdpPack>  messageQueue;
 
    private postboxUdp()
    {   //큐 초기화
        messageQueue = new Queue<UdpPack>();
    }
 
    //큐에 데이타 삽입
    public void PushData(UdpPack data)
    {
        messageQueue.Enqueue(data);
    }
 
    //큐에있는 데이타 꺼내서 반환
    public UdpPack GetData()
    {
        //데이타가 1개라도 있을 경우 꺼내서 반환
        if (messageQueue.Count > 0)
            return messageQueue.Dequeue();
        else
            return new UdpPack();    //없으면 빈값을 반환
    }
}
