using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace LosPinotsWebServicesSDK
{
    public abstract class Http : MonoBehaviour
    {
        private const string ApiUrl = "https://yourdomain.com/api/"; //SERVER DOMAIN
        public string route;
        private Uri Uri => new Uri(ApiUrl + route);

        protected WWWForm form;
    
        [SerializeField]protected Method method;
        [SerializeField]protected bool needsAuth;
        public bool taskEnd;

        private const int MaxErrorCount = 5;
        private int _errorCount = 0;

        public delegate void OnRequestSend();
        public event OnRequestSend Success, Fail;

        protected enum Method
        {
            Get,
            Post
        }
        public void MakeCall()
        {
            switch (method)
            {
                case Method.Post:
                    form = new WWWForm();
                    SetUpForm();
                    StartCoroutine(SendPost());
                    break;
                case Method.Get:
                    StartCoroutine(SendGet());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private IEnumerator SendGet()
        {
            taskEnd = false;

            using (var www = UnityWebRequest.Get(Uri))
            {
                www.SetRequestHeader("Accept", "application/json");
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                if (needsAuth)
                {
                    www.SetRequestHeader("Authorization", "Bearer " + UserData.Instance.GetToken());
                }
                Debug.Log("Getting data from: " + www.uri);
                yield return www.SendWebRequest();
            
                var jsonString = www.downloadHandler.text;
            
                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(jsonString);
                    FillData(jsonString);
                
                    Success?.Invoke();
                }
                else if(!string.IsNullOrEmpty(www.error))
                {
                    _errorCount++;
                    if (_errorCount >= MaxErrorCount)
                    {
                        Debug.LogError(www.error);
                        Debug.LogError(jsonString);

                        Fail?.Invoke();
                        yield break;
                    }
                    else
                    {
                        StartCoroutine(SendGet());
                        yield break;
                    }
                }
            }
            taskEnd = true;
        }
        private IEnumerator SendPost()
        {
            taskEnd = false;

            using (var www = UnityWebRequest.Post(Uri, form))
            {
                www.SetRequestHeader("Accept", "application/json");
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                if (needsAuth)
                {
                    www.SetRequestHeader("Authorization", "Bearer " + UserData.Instance.GetToken());
                }
                Debug.Log("Sending: " + Encoding.Default.GetString(form.data) + " to: " + www.uri);
                yield return www.SendWebRequest();
            
                var jsonString = www.downloadHandler.text;
            
                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(jsonString);
                    FillData(jsonString);
                
                    Success?.Invoke();
                }
                else if(!string.IsNullOrEmpty(www.error))
                {
                    _errorCount++;
                    if (_errorCount >= MaxErrorCount)
                    {
                        Debug.LogError(www.error);
                        Debug.LogError(jsonString);

                        Fail?.Invoke();
                        yield break;
                    }
                    else
                    {
                        StartCoroutine(SendPost());
                        yield break;
                    }
                }
            }
            taskEnd = true;
        }
        protected abstract void SetUpForm();
        protected abstract void FillData(string jsonString);
    }
}
