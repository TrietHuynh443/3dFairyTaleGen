using System;
using EventProcessing;
using SceneEnity;
using UnityEngine;

public class PlayerAudioTrigger : MonoBehaviour
{
    private AudioSource _currentAudioSource;

    private void OnEnable()
    {
        EventAggregator.Instance.AddEventListener<OnChangeParagraphEvent>(StopPlaying);
    }

    private void OnDisable()
    {
        EventAggregator.Instance?.RemoveEventListener<OnChangeParagraphEvent>(StopPlaying);
    }

    private void StopPlaying(OnChangeParagraphEvent obj)
    {
        if (_currentAudioSource != null)
        {
            _currentAudioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag($"character"))
        {
            Debug.Log(collision.gameObject.name + " triggered not play");
        }

        if (!InitialLoader.IsInitialized || InitialLoader.Instance.IsCurrentAudioPlaying || !collision.gameObject.CompareTag($"character"))
        {
            return;
        }
        Debug.Log(collision.gameObject.name + " triggered");
        if (!_currentAudioSource)
        {
            _currentAudioSource = collision.gameObject.GetComponent<AudioSource>();
        }

        if (!_currentAudioSource.isPlaying)
        {
            Debug.Log(collision.gameObject.name + " triggered");
            _currentAudioSource.Play();
        }



    }
}
