using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ButtonType
{
    Multiplay,
    Option,
    Sound,
    Description,
    Back_option,
    Back_multiplay,
    Back_description,
    Quit
}
public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public ButtonType curType;
    Vector3 defaultScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public CanvasGroup multiplayGroup;
    public CanvasGroup descriptionGroup;
    
    Text soundText;
    public static bool isSound = true;

    private void Start()
    {
        defaultScale = transform.parent.localScale;
        if(this.curType == ButtonType.Sound)
        {
            soundText = gameObject.GetComponentInChildren<Text>();
        }
    }
    public void OnBtnClick()
    {
        switch(curType)
        {
            case ButtonType.Multiplay:
                CanvasGroupOn(multiplayGroup);
                CanvasGroupOff(mainGroup);
                Debug.Log("멀티플레이");
                //SceneManager.LoadScene("MainScene");
                break;
            case ButtonType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case ButtonType.Sound:
                if(isSound)
                {
                    isSound = !isSound;
                    soundText.text = "사운드 X";
                }
                else
                {
                    isSound = !isSound;
                    soundText.text = "사운드 O";
                }
                break;
             case ButtonType.Description:
                CanvasGroupOn(descriptionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case ButtonType.Back_option:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case ButtonType.Back_multiplay:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(multiplayGroup);
                break;
            case ButtonType.Back_description:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(descriptionGroup);
                break;
            case ButtonType.Quit:
                Application.Quit();
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.localScale = defaultScale * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.localScale = defaultScale;
    }
}
