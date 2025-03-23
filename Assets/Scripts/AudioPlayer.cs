using Cinemachine;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;

    private CinemachineVirtualCamera _vcam;
    private bool _isPlay = false;
    private bool _forcedPlay = false;

    private void Start()
    {
        _vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!InitialLoader.IsInitialized)
            return;
        // Check if this virtual camera is the active one
        if (_vcam != null && CinemachineCore.Instance.IsLive(_vcam))
        {
            if (!_audio.isPlaying && !_isPlay || _forcedPlay)
            {
                _isPlay = true;
                _audio.Play();
            }
        }
        else
        {
            _audio.Stop();
        }
    }
}