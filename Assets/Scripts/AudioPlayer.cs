using System;
using System.Collections;
using Cinemachine;
using EventProcessing;
using UnityEngine;

namespace SceneEnity
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;
        public bool IsPlaying => _audio.isPlaying;


        public void ForcePlay()
        {
            StartCoroutine(DelayPlay());
        }

        private IEnumerator DelayPlay()
        {
            yield return new WaitForSeconds(1.5f);
            _audio.Play();
        }

        public void ForcedStop()
        {
            _audio.Stop();
        }

        public void ImmediatePlay()
        {
            _audio.Play();
        }
    }
}
