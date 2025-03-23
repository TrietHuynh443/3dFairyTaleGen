using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public static class NetworkHelper
    {
        public static async UniTask<string> DoGet(string fullUrl)
        {
            using var request = UnityWebRequest.Get(fullUrl);
            await request.SendWebRequest();
    
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                throw new System.Exception(request.error);
            }
        }
    
        public static async UniTask<string> DoPost(string fullUrl, byte[] rawData)
        {
            using var request = new UnityWebRequest(fullUrl, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(rawData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
    
            await request.SendWebRequest();
    
            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                throw new System.Exception(request.error);
            }
        }
    
        public static T DoParseJson<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
    
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private string _baseUrl = "http://localhost";
        [SerializeField] private int _port = 8080;
        
        public string Url => $"{_baseUrl}/{_port}";
        
    }

}
