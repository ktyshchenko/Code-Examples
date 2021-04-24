using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowInstructions : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject instr;

    public void OnPointerEnter(PointerEventData eventData)
    {
        instr.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        instr.SetActive(false);
    }
}
