using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BackpackItemUI : MonoBehaviour
{
    [SerializeField] private Image itemImageBackground;
    [SerializeField] private Image itemImage;

    public GraphicRaycaster canvasRaycaster;
    private Color defaultColor;

    public void SetItemIcon(bool value)
    {
        itemImageBackground.color = value ? Color.white : Color.green;
    }

    public void SetItem()
    {
        itemImage.color = defaultColor;
    }

    public void DetachFromBackpack()
    {
        itemImage.color = Color.gray;
    }

    private void Awake()
    {
        defaultColor = itemImage.color;
        itemImage.color = Color.gray;
    }
}
