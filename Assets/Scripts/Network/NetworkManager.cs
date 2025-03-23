using System.Collections.Generic;
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
        private string[] _paragraphs;
        private Dictionary<string, string[]> _paragraphDict = new Dictionary<string, string[]>();
        public string Url => $"{_baseUrl}:{_port}";

        public async UniTask<string[]> GetParagraphs(string story, string title)
        {
            if (_paragraphDict.TryGetValue(title, out var paragraphs))
            {
                return paragraphs;
            }
            var jsonRaw = await NetworkHelper.DoGet($"{Url}/split-story?story={story}");
            _paragraphs = NetworkHelper.DoParseJson<string[]>(jsonRaw);
            _paragraphDict.Add(title, _paragraphs);
            return _paragraphs;
        }
    }

}
