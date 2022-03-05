using UnityEngine;
using UnityEngine.InputSystem;

public class DragSystem : MonoBehaviour
{
    [HideInInspector]
    public BackpackItem selectedItem;

    [SerializeField]
    private float forceAmount = 500;

    private Camera targetCamera;
    private Vector3 originalScreenTargetPosition;
    private Vector3 originalRigidbodyPos;
    private float selectionDistance;

    private InputMaster inputMaster;
    private Raycaster raycaster;

    private void Start()
    {
        targetCamera = GetComponent<Camera>();

        raycaster = GameManager.instance.raycaster;
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();

        inputMaster.Player.Select.started += (InputAction.CallbackContext context) =>
        {
            SelectButtonDown();
        };

        inputMaster.Player.Select.canceled += (InputAction.CallbackContext context) =>
        {
            SelectButtonUp();
        };
    }

    private void SelectButtonDown()
    {
        var hitInfo = raycaster.GetHitInfoFromMouseClick<BackpackItem>();

        if (hitInfo.transform != null && !hitInfo.collider.gameObject.GetComponent<BackpackItem>().itemState.Equals(ItemState.InBackPack))
        {
            selectionDistance = Vector3.Distance(targetCamera.transform.position, hitInfo.point);

            var mousePosition = inputMaster.Player.CursorPosition.ReadValue<Vector2>();
            originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, selectionDistance));
            originalRigidbodyPos = hitInfo.collider.transform.position;
            selectedItem = hitInfo.collider.gameObject.GetComponent<BackpackItem>();
        }
    }

    private void SelectButtonUp()
    {
        selectedItem = null;
    }

    private void FixedUpdate()
    {
        if (selectedItem && selectedItem.GetComponent<BackpackItem>().itemState.Equals(ItemState.OutsideBackPack))
        {
            var mousePosition = inputMaster.Player.CursorPosition.ReadValue<Vector2>();
            Vector3 mousePositionOffset = targetCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
            selectedItem.itemRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedItem.transform.position) * forceAmount * Time.deltaTime;
        }
    }
}
