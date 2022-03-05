using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "webRequestConfig", menuName = "ScriptableObjects/WebRequestConfig")]
public class WebRequestConfig : ScriptableObject
{
    public string url;
    public string authKey;
}
