using System;
using System.Collections;
using Cinemachine;
using EventProcessing;
using UnityEngine;

namespace SceneEnity
{
    public class AudioPlayer : MonoBehaviour
    {
        static AudioSource _instance;
        [SerializeField] private AudioSource _audio;

        public void ForcePlay()
        {
            _instance = _audio;
            StartCoroutine(DelayPlay());
        }

        private IEnumerator DelayPlay()
        {
            yield return new WaitForSeconds(3.5f);
            if (_instance == _audio)
            {
                _audio.Play();
            }
        }

        public void ForcedStop()
        {
            _audio.Stop();
        }

    }
}
