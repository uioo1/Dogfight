using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    Vector3 defaultScale;
    public CanvasGroup firstCanvas;
    private CanvasGroup currentCanvas;
    void Start()
    {
        currentCanvas = firstCanvas;
        defaultScale = transform.parent.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBtnClick(CanvasGroup cg)
    {
        CanvasGroupOff(currentCanvas);
        currentCanvas = cg;
        CanvasGroupOn(currentCanvas);
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
