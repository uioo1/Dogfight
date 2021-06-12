using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RoomLabel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public Text indexText;
    public Text nameText;
    public Text lockText;
    public Text statusText;
    Vector3 defaultScale;
    public int index;
    public int isLocked;
    GameObject clientControl;
    void Start(){
        
        defaultScale = transform.localScale;
        clientControl = GameObject.Find("ClientManager");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = defaultScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = defaultScale;
    }
    public void clicked(){
        clientControl.GetComponent<zzzClient>().accessRoom(index);
    }
}
