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

  

        public void ForcePlay()
        {
            _audio.Play();
        }

        public void ForcedStop()
        {
            _audio.Stop();
        }

    }
}
