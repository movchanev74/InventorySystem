using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BackpackItemHolder : MonoBehaviour
{
    public ItemTypes itemType;
    public BackpackItemUI itemUI;

    private BackpackItem item;
    private InputMaster inputMaster;
    private List<RaycastResult> list;

    public void SetItem(BackpackItem item)
    {
        itemUI.SetItem();
        this.item = item;
    }

    public void RemoveItem()
    {
        itemUI.DetachFromBackpack();
        item = null;
    }

    private void Awake()
    {
        SubscribeToInput();
    }

    private void SubscribeToInput()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();

        inputMaster.Player.Select.canceled += (InputAction.CallbackContext context) =>
        {
            list = new List<RaycastResult>();
            PointerEventData ed = new PointerEventData(null)
            {
                position = inputMaster.Player.CursorPosition.ReadValue<Vector2>()
            };

            itemUI.canvasRaycaster.Raycast(ed, list);

            if (list != null && list.Where(x => x.gameObject == itemUI.gameObject).Count() > 0)
            {
                if (item != null)
                {
                    item.RemoveFromBackpack();
                }
            }
        };
    }
}
