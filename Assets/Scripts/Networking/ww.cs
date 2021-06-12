using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ww : MonoBehaviour
{
    // Start is called before the first frame update
    postbox p;    
    const int CMD_SEND_CHAT = 1;       // TO SEND CHAT
    const int CMD_RECV_ROOM = 2;       // TO RECV ROOM INFO
    const int CMD_MAKE_ROOM = 3;       // TO MAKE ROOM
    const int CMD_ACCE_ROOM = 4;       // TO ACCESS ROOM
    const int CMD_UPDATE_ROOM=5;       // TO UPDATE ROOM INFO
    const int CMD_RETURN_ROOM=6;       // SERVER SEND RETURN OF CLIENT's CMD
    const int CMD_CLOSE     = 7;       // TO CLOSE CONNECTION WITH SERVER
    
    public Text outputChat;
    public GameObject content;
    public GameObject label;
    void Start()
    {
        p = postbox.GetInstance;
    }

    // Update is called once per frame
    void Update()
    {
        packet data = p.GetData();
 
            //우편함에 데이타가 있는 경우
        if (!data.Equals(new packet()))
        {
            //데이타로 UI 갱신
            ResponseData(data);
        }
    }
    public void ResponseData(packet d)
    {
        switch ((int)d.cmd)
        {
            case CMD_SEND_CHAT :
                for(int i = 0; i < 1024; i++){
                    if (d.data[i] == (char)0)
                        break;

                    if(d.data[i] == '\\'){
                        outputChat.text += " : ";
                    }
                    else{
                        outputChat.text += d.data[i];
                    }
                }
                outputChat.text += '\n';
            break;
            
            case CMD_RECV_ROOM :
                string[] a = d.data.Split('\\');

                GameObject gameObject = Instantiate(label, content.transform);
                gameObject.GetComponent<RoomLabel>().isLocked = int.Parse(a[1]);
                gameObject.GetComponent<RoomLabel>().index = int.Parse(a[0]);

                gameObject.GetComponent<RoomLabel>().indexText.text = a[0];
                gameObject.GetComponent<RoomLabel>().lockText.text = a[1];
                gameObject.GetComponent<RoomLabel>().nameText.text = a[2];
                gameObject.GetComponent<RoomLabel>().statusText.text = a[3];
            break;

            default:
            break;
        }
    }

    public void refreshContent(){
        foreach (Transform child in content.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
    private IEnumerator CheckQueue()
    {
        //1초 주기로 탐색
        WaitForSeconds waitSec = new WaitForSeconds(1);
        while (true)
        {
            //우편함에서 데이타 꺼내기
            
 
            yield return waitSec;
        }
    }


    
}
