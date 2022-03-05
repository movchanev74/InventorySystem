using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum ItemState
{
    InBackPack,
    OutsideBackPack,
    TryOnInBackPack,
}

public enum ItemTypes
{
    Box,
    Prisma,
    Hex
}

[RequireComponent(typeof(Rigidbody))]
public class BackpackItem : MonoBehaviour
{
    [HideInInspector]
    public ItemTypes itemType;

    public BackpackItemConfig config;

    private Vector3 oldPosition;
    private BackpackItemHolder backpackItemHolder;
    private Sequence sequence;
    private InputMaster inputMaster;

    public Rigidbody itemRigidbody
    {
        get; private set;
    }

    public ItemState itemState
    {
        get; private set;
    } = ItemState.OutsideBackPack;

    public UnityEvent OnAttachToBackpack;
    public UnityEvent OnDetachFromBackpack;

    public void InsertInBackpack(BackpackItemHolder itemHolder)
    {
        backpackItemHolder = itemHolder;

        itemState = ItemState.TryOnInBackPack;
        itemRigidbody.useGravity = false;
        itemRigidbody.isKinematic = true;

        oldPosition = transform.position;
        MoveToPoint(backpackItemHolder.transform.position, backpackItemHolder.transform.rotation, backpackItemHolder.transform.localScale);
    }

    public void RemoveFromBackpack()
    {
        backpackItemHolder = null;

        if (itemState.Equals(ItemState.InBackPack))
        {
            OnDetachFromBackpack?.Invoke();
        }
        itemState = ItemState.OutsideBackPack;

        MoveToPoint(oldPosition, transform.rotation, Vector3.one * 0.1f, delegate
        {
            itemRigidbody.useGravity = true;
            itemRigidbody.isKinematic = false;
        });
    }

    private void MoveToPoint(Vector3 position, Quaternion rotation, Vector3 scale, Action onColmplete = null)
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(transform.DORotateQuaternion(rotation, 0.4f));
        sequence.Join(transform.DOMove(position, 0.4f));
        sequence.Join(transform.DOScale(scale, 0.4f));
        sequence.OnComplete(delegate
        {
            onColmplete?.Invoke();
        });
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Enable();

        inputMaster.Player.Select.canceled += (InputAction.CallbackContext context) =>
        {
            SelectButtonUp();
        };

        itemRigidbody = GetComponent<Rigidbody>();
        itemType = config.itemType;
        itemRigidbody.mass = config.itemMass;

        var webRequestSender = GameManager.instance.webRequestSender;

        OnAttachToBackpack.AddListener(() => { webRequestSender.SendPostRequest(this); });
        OnDetachFromBackpack.AddListener(() => { webRequestSender.SendPostRequest(this); });
    }

    private void SelectButtonUp()
    {
        if (itemState.Equals(ItemState.TryOnInBackPack))
        {
            itemState = ItemState.InBackPack;
            OnAttachToBackpack?.Invoke();
        }
    }
}