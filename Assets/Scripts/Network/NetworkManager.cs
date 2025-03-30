using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using SO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

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


        public static async UniTask<MemoryStream> DoGetStreaming(string fullUrl)
        {
            using var request = UnityWebRequest.Get(fullUrl);
            request.downloadHandler = new DownloadHandlerBuffer(); // Efficient memory handling

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success || request.downloadHandler.data == null)
            {
                throw new System.Exception(request.error);
            }

            return new MemoryStream(request.downloadHandler.data, writable: false);
        }
        
        public static async UniTask<AudioClip> DoDownloadAudioClip(string url, AudioType audioType)
        {
            using var request = UnityWebRequestMultimedia.GetAudioClip(url, audioType);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception($"Audio download failed: {request.error}");
            }

            return DownloadHandlerAudioClip.GetContent(request);
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
        [SerializeField] private EnvironmentSO _config;
        private string[] _paragraphs;
        private Dictionary<string, string[]> _paragraphDict = new Dictionary<string, string[]>();
        
        private static Stopwatch _timer;
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

        public async UniTask<AudioClip[]> GetAudioClip(string story, string title)
        {
            _timer = Stopwatch.StartNew();
            AudioClip[] audioClips;

            if (_config != null && _config.environmentType == EnvironmentType.Local)
            {
                audioClips = Resources.LoadAll<AudioClip>($"{title}/Audio");
                return audioClips;
            }
            if (_paragraphDict.TryGetValue(title, out var paragraphs))
            {
                _paragraphs = paragraphs;
            }
            
            _paragraphs = await GetParagraphs(story, title);
            audioClips = new AudioClip[_paragraphs.Length];

            for (int i = 0; i < _paragraphs.Length; i++)
            {
                AudioClip audioClip = await NetworkHelper.DoDownloadAudioClip($"{Url}/stream-audio?text={_paragraphs[i]}", AudioType.WAV);
                audioClips[i] = audioClip;
#if UNITY_EDITOR
                // Ensure the directory exists
                var savePath = Path.Combine(Application.dataPath, "Resources", title, "Audio");
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                // Convert AudioClip to WAV and save in Resources
                string filePath = Path.Combine(savePath, $"{i}.wav");
                SaveAudioClip(audioClip, filePath);
#endif
            }

            Debug.Log("Loaded Audio elapsed time " + _timer.Elapsed.TotalSeconds + " seconds");
            return audioClips;
        }
        
#if UNITY_EDITOR
        private void SaveAudioClip(AudioClip clip, string path)
        {
            byte[] wavData = WavUtility.ToWav(clip); // Convert AudioClip to WAV bytes (you need a WavUtility class)
            File.WriteAllBytes(path, wavData);
            AssetDatabase.Refresh(); // Refresh Unity Editor to recognize the new file
            Debug.Log($"Saved audio: {path}");
        }
#endif
    }
}
