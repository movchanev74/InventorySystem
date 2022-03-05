using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public DragSystem dragSystem;
    public Raycaster raycaster;
    public WebRequestSender webRequestSender;

    [SerializeField] private WebRequestConfig webRequestConfig;

    private void Awake()
    {
        instance = this;

        webRequestSender = new WebRequestSender(webRequestConfig);
    }
}
