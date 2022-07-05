using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Omnilatent.AdjustUnity
{
    public class LocationUtils
    {
        [Serializable]
        public class IpApiData
        {
            public string countryCode;

            public static IpApiData CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<IpApiData>(jsonString);
                //return LitJson.JsonMapper.ToObject<IpApiData>(jsonString);
            }
        }

        public static async void GetUserCountryAsync(Action<IpApiData> onGetDataHandler)
        {
            //string ip = new System.Net.WebClient().DownloadString("https://api.ipify.org");
            string uri = $"http://ip-api.com/json";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                bool timedOut = false;
                var asyncRequest = webRequest.SendWebRequest();
                asyncRequest.completed += (AsyncOperation operation) =>
                {
                    string[] pages = uri.Split('/');
                    int page = pages.Length - 1;

                    IpApiData ipApiData = IpApiData.CreateFromJSON(webRequest.downloadHandler.text);
                    //Debug.Log(webRequest.downloadHandler.text);
                    //Debug.Log(ipApiData.countryCode);
                    if (!timedOut)
                        onGetDataHandler.Invoke(ipApiData);
                };
                await Task.Delay(3000);
                if (!asyncRequest.isDone)
                {
                    timedOut = true;
                    onGetDataHandler.Invoke(null);
                }
            }
        }
    }
}