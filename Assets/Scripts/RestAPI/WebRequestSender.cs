using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestSender
{
    [Serializable]
    struct PostRequest
    {
        public string auth;
        public string id;
        public string action;
    }

    private WebRequestConfig config;

    public WebRequestSender(WebRequestConfig config)
    {
        this.config = config;
    }

    public void SendPostRequest(BackpackItem item)
    {
        _ = SendPostRequestAsync(config.url, CreateJson(item));
    }

    public string CreateJson(BackpackItem item)
    {
        var postRequest = new PostRequest()
        {
            auth = config.authKey,
            id = item.config.itemID,
            action = item.itemState.ToString()
        };

        return JsonUtility.ToJson(postRequest);
    }

    public async UniTask SendPostRequestAsync(string url, string json)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, json))
        {
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
