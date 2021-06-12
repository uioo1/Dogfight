using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class postbox {
    //싱글턴 인스턴스
    private static postbox instance;
    //싱글턴 인스턴스 반환
    public static postbox GetInstance
    {
        get
        {
            if (instance == null)
                instance = new postbox();
 
            return instance;
        }
    }
 
    //데이타를 담을 큐
    private Queue<packet>  messageQueue;
 
    private postbox()
    {   //큐 초기화
        messageQueue = new Queue<packet>();
    }
 
    //큐에 데이타 삽입
    public void PushData(packet data)
    {
        messageQueue.Enqueue(data);
    }
 
    //큐에있는 데이타 꺼내서 반환
    public packet GetData()
    {
        //데이타가 1개라도 있을 경우 꺼내서 반환
        if (messageQueue.Count > 0)
            return messageQueue.Dequeue();
        else
            return new packet();    //없으면 빈값을 반환
    }
}
