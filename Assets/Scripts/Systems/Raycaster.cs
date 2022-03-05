using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private Camera targetCamera;
    private InputMaster inputMaster;

    private void Start()
    {
        targetCamera = GetComponent<Camera>();
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();
    }

    public RaycastHit GetHitInfoFromMouseClick<T>() where T : MonoBehaviour
    {
        Ray ray = targetCamera.ScreenPointToRay(inputMaster.Player.CursorPosition.ReadValue<Vector2>());
        bool hit = Physics.Raycast(ray, out RaycastHit hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<T>())
            {
                return hitInfo;
            }
        }

        return new RaycastHit();
    }
}
