using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Backpack : MonoBehaviour
{
    [SerializeField] private BackpackUI backpackUI;
    [SerializeField] private List<BackpackItemHolder> itemHolders;

    private InputMaster inputMaster;
    private bool isSelectPressed;
    private Raycaster raycaster;
    private DragSystem dragSystem;

    private void Start()
    {
        raycaster = GameManager.instance.raycaster;
        dragSystem = GameManager.instance.dragSystem;
        SubscribeToInput();
    }

    private void SubscribeToInput()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();

        inputMaster.Player.Select.started += (InputAction.CallbackContext context) =>
        {
            var hitInfo = raycaster.GetHitInfoFromMouseClick<Backpack>();

            if (hitInfo.collider != null && hitInfo.collider.gameObject == gameObject)
            {
                backpackUI.Open();
            }
            isSelectPressed = true;
        };

        inputMaster.Player.Select.canceled += (InputAction.CallbackContext context) =>
        {
            isSelectPressed = false;
            backpackUI.Hide();
        };
    }

    private void Update()
    {
        if(isSelectPressed)
        {
            if (dragSystem.selectedItem != null)
            {
                var hitInfo = raycaster.GetHitInfoFromMouseClick<Backpack>();

                if (hitInfo.collider != null && hitInfo.collider.gameObject == gameObject)
                {
                    SetToBackpack(dragSystem.selectedItem.GetComponent<BackpackItem>());
                }
                else
                {
                    GetFromBackpack(dragSystem.selectedItem.GetComponent<BackpackItem>());
                }
            }
        }
    }

    private void GetFromBackpack(BackpackItem backpackItem)
    {
        if (!backpackItem.itemState.Equals(ItemState.OutsideBackPack))
        {
            backpackItem.RemoveFromBackpack();
        }
    }

    private void SetToBackpack(BackpackItem backpackItem)
    {
        if (!backpackItem.itemState.Equals(ItemState.InBackPack))
        {
            var itemHolder = itemHolders.FirstOrDefault(x => x.itemType.Equals(backpackItem.itemType));
            if (itemHolder)
            {
                itemHolder.SetItem(backpackItem);
                backpackItem.InsertInBackpack(itemHolder);
            }
        }
    }
}
