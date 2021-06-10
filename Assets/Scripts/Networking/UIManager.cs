using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject multiplayMenu;
    public InputField useernamefield;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Debug.Log("Instance already exists, destory object");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        multiplayMenu.SetActive(false);
        useernamefield.interactable = false;
        Client.instance.ConnectToServer();
    }
}
